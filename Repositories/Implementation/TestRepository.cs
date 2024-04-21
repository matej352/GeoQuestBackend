using GeoQuest.DTOs;
using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoQuest.Repositories.Implementation
{
    public class TestRepository : ITestRepository
    {

        private readonly GeoQuestContext _context;

        public TestRepository(GeoQuestContext context)
        {
            _context = context;
        }


        public async Task<Test> GetTest(int id)
        {
            var _test = await _context.Test.Include(t => t.Subject).FirstOrDefaultAsync(t => t.Id == id);

            if (_test is null)
            {
                throw new Exception($"Test with id = {id} does not exists");
            }
            else
            {
                return _test;
            }
        }

        public async Task<IEnumerable<Test>> GetTests(int teacherId)
        {
            var tests = await _context.Test.Include(t => t.Subject).Where(t => t.TeacherId == teacherId).ToListAsync();
            return tests;
        }

        public async Task PublishTest(int testId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Create a TestInstanceBase for the published test
                    var testInstanceBase = new TestInstanceBase
                    {
                        InstancesCount = 0, // Initialize to 0, assuming it will be incremented later
                        Active = true, // Assuming the test instance base is active upon publishing
                        TestId = testId // Assign the testId to link the test instance base with the test
                    };

                    _context.TestInstanceBase.Add(testInstanceBase);
                    await _context.SaveChangesAsync();



                    var test = await _context.Test.Include(t => t.TestTask).Where(t => t.Id == testId).FirstOrDefaultAsync();
                    if (test is null)
                    {
                        throw new Exception($"Test with id = {testId} does not exists");
                    }

                    // 2. Retrieve all students associated with the subject of the test
                    var subjectStudents = _context.Test.Include(t => t.Subject)
                        .Where(t => t.Id == testId)
                        .SelectMany(t => t.Subject.Student)
                        .ToList();

                    // 3. Create a TestInstance for each student and assign it the appropriate TestTaskInstances
                    foreach (var student in subjectStudents)
                    {
                        var testInstance = new TestInstance
                        {

                            Started = false, // Assuming the test instance is not opened initially
                            Finished = false,
                            TestInstanceBaseId = testInstanceBase.Id, // Assign the test instance base id
                            StudentId = student.Id // Assign the student id
                        };

                        _context.TestInstance.Add(testInstance);
                        await _context.SaveChangesAsync();

                        // Create TestTaskInstances for the student
                        var testTasks = test.TestTask.ToList();
                        foreach (var task in testTasks)
                        {
                            var testTaskInstance = new TestTaskInstance
                            {
                                StudentAnswer = null, // Assuming the student hasn't answered initially
                                Correct = false, // Initialize correctness to false
                                Checked = false, // Assuming the task instance is not checked initially
                                TestTaskId = task.Id, // Assign the test task id
                                TestInstanceId = testInstance.Id // Assign the test instance id
                            };

                            _context.TestTaskInstance.Add(testTaskInstance);
                            await _context.SaveChangesAsync();
                        }


                    }

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception($"Problem while publishing test with ID {testId}! " + exception.Message);
                }
            }
        }

        public async Task<int> SaveTest(CreateTestDto test, int teacherId)
        {
            Test newTest = new Test
            {
                Name = test.Name,
                Duration = test.Duration,
                Description = test.Description,
                SubjectId = test.SubjectId,
                TeacherId = teacherId,
            };

            _context.Add(newTest);
            await _context.SaveChangesAsync();

            return newTest.Id;
        }



    }
}
