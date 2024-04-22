using GeoQuest.DTOs;
using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoQuest.Repositories.Implementation
{
    public class TestInstanceRepository : ITestInstanceRepository
    {

        private readonly GeoQuestContext _context;

        public TestInstanceRepository(GeoQuestContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<TestInstance>> GetTestInstances(int studentId, bool finished)
        {

            var testInstances = new List<TestInstance>();

            if (finished)
            {
                testInstances = await _context.TestInstance
                .Include(t => t.TestInstanceBase.Test)
                .Include(t => t.TestInstanceBase.Test.Teacher)
                .Include(t => t.TestInstanceBase.Test.Subject)
                .Where(t => t.StudentId == studentId && t.Started == true && t.Finished == true).ToListAsync();
            }
            else
            {
                testInstances = await _context.TestInstance
               .Include(t => t.TestInstanceBase.Test)
               .Include(t => t.TestInstanceBase.Test.Teacher)
               .Include(t => t.TestInstanceBase.Test.Subject)
               .Where(t => t.StudentId == studentId && t.Started == false && t.Finished == false && t.TestInstanceBase.Active == true).ToListAsync();       // dohvati samo one instance koje jos nisu pokrenute i koje nisu rijesene i koje se jos uvijek mogu pokrenuti (učitelj nije zatvorio ispit)
            }

            return testInstances;
        }

        public async Task StartTestInstance(int instanceId, int studentId)
        {
            var instance = await _context.TestInstance.Where(t => t.StudentId == studentId && t.Id == instanceId).FirstOrDefaultAsync();

            if (instance == null)
            {
                throw new Exception($"Test instance with id = {instanceId} does not exist for student with id = {studentId}");
            }

            instance.Started = true;
            await _context.SaveChangesAsync();
        }

        public async Task FinishTestInstance(int instanceId, TimeSpan elapsedTime, int studentId)
        {
            var instance = await _context.TestInstance.Where(t => t.StudentId == studentId && t.Id == instanceId).FirstOrDefaultAsync();

            if (instance == null)
            {
                throw new Exception($"Test instance with id = {instanceId} does not exist for student with id = {studentId}");
            }

            instance.Finished = true;
            instance.ElapsedTime = elapsedTime;
            await _context.SaveChangesAsync();
        }

        public async Task<TestInstance> GetTestInstance(int testInstanceId)
        {

            //treba i provjera da učenik usercontext.id hoce doci do instance koja njemu pripada

            var instance = await _context.TestInstance
                .Include(t => t.TestInstanceBase.Test)
                .Where(t => t.Id == testInstanceId && t.Finished == false && t.TestInstanceBase.Active == true)
                .FirstOrDefaultAsync();       // dohvati instancu ako jos nije rijesena i ako se jos uvijek moze pokrenuti (učitelj nije zatvorio ispit)


            if (instance == null)
            {
                throw new Exception($"Ispit je ili rijesen ili je učitelj vec zatvorio ispit");
            }

            return instance;
        }

        public async Task UpdateElapsedTime(int instanceId, TimeSpan elapsedTime, int studentId)
        {
            var instance = await _context.TestInstance
               .Include(t => t.TestInstanceBase.Test)
               .Where(t => t.Id == instanceId && t.Finished == false && t.TestInstanceBase.Active == true && t.StudentId == studentId)
               .FirstOrDefaultAsync();


            if (instance == null)
            {
                throw new Exception($"Problem while updating elapsed time for test instance with id = {instanceId}");
            }

            instance.ElapsedTime = elapsedTime;
            await _context.SaveChangesAsync();
        }

        public async Task<TestInstanceResultDto> GetTestInstanceResult(int testInstanceId)
        {


            var testTaskInstances = await _context.TestTaskInstance
                  .Where(tti => tti.TestInstanceId == testInstanceId)
                  .Select(tti => new
                  {
                      tti.StudentAnswer,
                      CorrectAnswer = tti.TestTask.Answer != "" ? tti.TestTask.Answer :
                          _context.OptionAnswer
                              .Where(oa => oa.OptionId == tti.TestTask.OptionsId && oa.Correct)
                              .Select(oa => oa.Content)
                              .FirstOrDefault()
                  })
                  .ToListAsync();



            var testInstanceResultDto = await _context.TestInstance
                                            .Where(ti => ti.Id == testInstanceId)
                                            .Select(ti => new TestInstanceResultDto
                                            {
                                                TestInstanceId = ti.Id,
                                                TotalPoints = ti.TestTaskInstance.Count(tt => tt.Correct),    // Each correct task is 1 point
                                                PercentageResolved = (double)ti.TestTaskInstance.Count(tt => tt.Checked) / ti.TestTaskInstance.Count() * 100,
                                                TestTasks = ti.TestTaskInstance.Select(tti => new TestTaskResultDto
                                                {
                                                    TestTaskId = tti.TestTaskId,
                                                    Question = tti.TestTask.Question,
                                                    CorrectAnswer = tti.TestTask.OptionsId != null ?
                                                        _context.OptionAnswer
                                                            .Where(oa => oa.OptionId == tti.TestTask.OptionsId && oa.Correct)
                                                            .Select(oa => oa.Id.ToString())
                                                            .FirstOrDefault() :
                                                        tti.TestTask.Answer,
                                                    Options = tti.TestTask.OptionsId != null ? new OptionsDto
                                                    {
                                                        Id = tti.TestTask.OptionsId.Value,
                                                        SingleSelect = tti.TestTask.Options.SingleSelect,
                                                        OptionAnswers = _context.OptionAnswer
                                                            .Where(oa => oa.OptionId == tti.TestTask.OptionsId)
                                                            .Select(oa => new OptionAnswerDto
                                                            {
                                                                Id = oa.Id,
                                                                Content = oa.Content,
                                                                Correct = oa.Correct
                                                            }).ToList()
                                                    } : null,
                                                    StudentAnswer = tti.StudentAnswer,
                                                    IsCorrect = tti.Correct
                                                }).ToList()
                                            })
                                            .FirstOrDefaultAsync();


            return testInstanceResultDto;


        }

    }
}
