using GeoQuest.DTOs;
using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GeoQuest.Repositories.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly GeoQuestContext _context;

        public TaskRepository(GeoQuestContext context)
        {
            _context = context;
        }

        public async Task<TestTask> GetTask(int id)
        {
            var _task = await _context.TestTask.Include(t => t.Test).Include(t => t.Options).Include(t => t.Options.OptionAnswer).FirstOrDefaultAsync(t => t.Id == id);

            if (_task is null)
            {
                throw new Exception($"Task with id = {id} does not exists");
            }
            else
            {
                return _task;
            }
        }

        public async Task<IEnumerable<TestTask>> GetTasks(int testId)
        {
            var test = await _context.Test.FirstOrDefaultAsync(t => t.Id == testId);
            if (test is null)
            {
                throw new Exception($"Test with id = {testId} does not exists");
            }

            var _tasks = await _context.TestTask.Include(t => t.Test).Include(t => t.Options).Include(t => t.Options.OptionAnswer).Where(t => t.TestId == testId).ToListAsync();

            return _tasks;

        }

        public async Task<int> SaveTask(TaskDto taskDto, int testId)
        {

            var test = await _context.Test.FirstOrDefaultAsync(t => t.Id == testId);

            if (test is null)
            {
                throw new Exception($"Test with id = {testId} does not exists");
            }

            // Convert TaskDto to Task entity
            var taskEntity = new TestTask
            {
                MapCenter = taskDto.MapCenter,
                MapType = (int)taskDto.MapType,
                MapZoomLevel = taskDto.MapZoomLevel,
                Question = taskDto.Question,
                Answer = taskDto.Answer ?? null,
                NonMapPoint = taskDto.NonMapPoint ?? null,
                Type = (int)taskDto.Type,
                TestId = test.Id,
                // Do not set OptionsId here; it will be set upon saving Options
            };

            // Check if taskDto has options
            if (taskDto.Options != null)
            {
                var optionsEntity = new Options
                {
                    SingleSelect = taskDto.Options.SingleSelect,
                    OptionAnswer = taskDto.Options.OptionAnswers.Select(oa => new OptionAnswer
                    {
                        Content = oa.Content,
                        Correct = oa.Correct
                    }).ToList()
                };

                // Save Options first to get an Id
                await _context.Options.AddAsync(optionsEntity);
                await _context.SaveChangesAsync();

                // Link the Options to the Task
                taskEntity.OptionsId = optionsEntity.Id;
            }

            // Now save the Task
            await _context.TestTask.AddAsync(taskEntity);
            await _context.SaveChangesAsync();

            // Assuming you have a mechanism to associate this task with a test
            // This might involve updating a join table or setting a property on the task
            // For example, if a TestTask join table exists:
            // _context.TestTasks.Add(new TestTask { TestId = testId, TaskId = taskEntity.Id });
            // await _context.SaveChangesAsync();






            return taskEntity.Id;
        }
    }
}
