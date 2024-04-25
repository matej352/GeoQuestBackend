using GeoQuest.DTOs;
using GeoQuest.Enums;
using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoQuest.Repositories.Implementation
{
    public class TestTaskInstanceRepository : ITestTaskInstanceRepository
    {

        private readonly GeoQuestContext _context;

        public TestTaskInstanceRepository(GeoQuestContext context)
        {
            _context = context;
        }




        public async Task<IEnumerable<TaskInstanceDto>> GetOnGoingTestTaskInstances(int testInstanceId, int studentId)
        {

            var instance = await _context.TestInstance.Where(t => t.StudentId == studentId && t.Id == testInstanceId).FirstOrDefaultAsync();

            if (instance == null)
            {
                throw new Exception($"Test instance with id = {testInstanceId} does not exist for student with id = {studentId}");

            }


            return await _context.TestTaskInstance
                .Include(tti => tti.TestTask)
                .Include(tti => tti.TestTask.Options)
                .Include(tti => tti.TestTask.Options.OptionAnswer)
                    .Where(tti => tti.TestInstanceId == testInstanceId)
                    .Select(tti => new TaskInstanceDto
                    {
                        Id = tti.Id,
                        Question = tti.TestTask.Question,
                        Answer = tti.StudentAnswer,
                        NonMapPoint = tti.TestTask.NonMapPoint,
                        Type = (TaskType)tti.TestTask.Type,
                        TestInstanceId = tti.TestInstanceId,
                        Options = tti.TestTask.Options != null ? new TaskInstanceOptionsDto
                        {
                            Id = tti.TestTask.Options.Id,
                            SingleSelect = tti.TestTask.Options.SingleSelect,
                            OptionAnswers = tti.TestTask.Options.OptionAnswer
                                .Select(option => new TaskInstanceOptionAnswerDto
                                {
                                    Id = option.Id,
                                    Content = option.Content
                                })
                                .ToList()
                        } : null
                    })
                    .ToListAsync();
        }

        public async Task SaveOnGoingTestTaskInstanceAnswer(TestTaskInstanceAnswerSaveDto saveAnswer, int studentId)
        {
            var taskInstance = await _context.TestTaskInstance
                .Include(tti => tti.TestInstance)
                .Where(tti => tti.TestInstanceId == saveAnswer.TestInstanceId && tti.Id == saveAnswer.TestTaskInstanceId && tti.TestInstance.StudentId == studentId)
                .FirstOrDefaultAsync();

            if (taskInstance == null)
            {
                throw new Exception("Problem with saving users answer");
            }

            taskInstance.StudentAnswer = saveAnswer.Answer; // u slučaju zadatka označi točkom/poligonom na karti, to će biti lat lng niz, u slučaju odabira od ponuđenog, to će biti Id opcije

            await _context.SaveChangesAsync();
        }
    }
}
