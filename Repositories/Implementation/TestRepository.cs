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
            var _test = await _context.Test
                .Include(t => t.Subject)
                .Include(t => t.Subject.Student)
                .Include(t => t.TestTask)
                .FirstOrDefaultAsync(t => t.Id == id);

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
            var tests = await _context.Test
                .Include(t => t.Subject)
                .Include(t => t.Subject.Student)
                .Include(t => t.TestTask)
                .Where(t => t.TeacherId == teacherId && t.Published == false)
                .ToListAsync();
            return tests;
        }



        public async Task<IEnumerable<TestPublishedDto>> GetPublishedTests(int teacherId)
        {
            var testPublishedDtos = await _context.TestInstanceBase
                                        .Include(t => t.Test)
                                        .Include(t => t.Test.Subject)
                                        .Where(t => t.Test.TeacherId == teacherId) // Filter by teacherId
                                        .Select(t => new TestPublishedDto
                                        {
                                            Id = t.Id,
                                            TeacherId = t.Test.TeacherId,
                                            Duration = t.Test.Duration,
                                            Name = t.Test.Name,
                                            Description = t.Test.Description,
                                            Subject = t.Test.Subject.Name, // Assuming you have a navigation property Subject
                                            FinishedByStudentInstanceCount = t.TestInstance.Count(ti => ti.Started && ti.Finished), // Count finished TestInstances
                                            InstanceCount = t.InstancesCount, // Total InstanceCount from TestInstanceBase
                                            Active = t.Active // Active status from TestInstanceBase
                                        })
                                       .ToListAsync();


            return testPublishedDtos;
        }



        public async Task<int> PublishTest(int testId)
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

                    if (test.Published)
                    {
                        throw new Exception($"Test with id = {testId} is already published!");
                    }

                    test.Published = true;
                    await _context.SaveChangesAsync();

                    // 2. Retrieve all students associated with the subject of the test
                    var subjectStudents = _context.Test.Include(t => t.Subject)
                        .Where(t => t.Id == testId)
                        .SelectMany(t => t.Subject.Student)
                        .ToList();


                    testInstanceBase.InstancesCount = subjectStudents.Count;
                    await _context.SaveChangesAsync();


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

                    return testInstanceBase.Id;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception($"Problem while publishing test with ID {testId}! " + exception.Message);
                }
            }
        }

        public async Task CloseTest(int testInstanceBaseId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    // Find the TestInstanceBase
                    var testInstanceBase = await _context.TestInstanceBase
                        .Include(tib => tib.TestInstance)
                        .ThenInclude(ti => ti.TestTaskInstance)
                        .Where(tib => tib.Id == testInstanceBaseId)
                        .FirstOrDefaultAsync();

                    if (testInstanceBase == null)
                    {
                        throw new Exception($"TestInstanceBase with id = {testInstanceBaseId} does not exist");
                    }

                    testInstanceBase.Active = false;
                    await _context.SaveChangesAsync();

                    // Kod zatvaranja ispita, svi ispiti studenata koji nisu ni pokrenuti, stavlja im se finished na true, svakom tasku tog ispita se stavlja checked na true, correct ostaje false
                    // Update unfinished TestInstances to finished
                    foreach (var testInstance in testInstanceBase.TestInstance.Where(ti => !ti.Finished))
                    {
                        testInstance.Finished = true;

                        // Update TestTaskInstances to checked
                        foreach (var testTaskInstance in testInstance.TestTaskInstance)
                        {
                            testTaskInstance.Checked = true;
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception($"Problem while closing TestInstanceBase with ID {testInstanceBaseId}! " + exception.Message);
                }
            }
        }

        public async Task<int> SaveTest(CreateTestDto test, int teacherId)
        {
            Test newTest = new Test
            {
                Name = test.Name,
                Duration = test.Duration,
                Description = test.Description ?? null,
                SubjectId = test.SubjectId,
                TeacherId = teacherId,
                Published = false
            };

            _context.Add(newTest);
            await _context.SaveChangesAsync();

            return newTest.Id;
        }

        public async Task<TestPublishedDetailsDto> GetPublishedTestOverview(int testInstanceBaseId)
        {
            var testInstanceBase = await _context.TestInstanceBase
                .Include(ti => ti.Test)
                    .ThenInclude(t => t.Subject)
                .FirstOrDefaultAsync(ti => ti.Id == testInstanceBaseId);

            if (testInstanceBase == null)
            {
                throw new Exception($"TestInstanceBase with id = {testInstanceBaseId} does not exists");
            }

            var testInstances = await _context.TestInstance
                .Include(ti => ti.TestTaskInstance)
                .Where(ti => ti.TestInstanceBaseId == testInstanceBaseId)
                .ToListAsync();

            // uzmi samo one instance koje su ucenici stvarno i pokrenuli i rijesili (jer se sa zatvaranjem ispita svim instancama iovako stavi finished=true)
            var finishedByStudentsInstanceCount = testInstances.Count(ti => ti.Finished && ti.Started);

            var finishedInstanceCount = testInstances.Count(ti => ti.Finished);
            var instanceCount = testInstances.Count;
            var active = testInstanceBase.Active;

            var avgElapsedTime = TimeSpan.Zero;
            if (finishedByStudentsInstanceCount > 0)
            {
                avgElapsedTime = TimeSpan.FromMilliseconds(testInstances.Where(ti => ti.Finished && ti.Started)
                    .Average(ti => ti.ElapsedTime?.TotalMilliseconds ?? 0));
            }

            var checkedInstanceCount = testInstances.Count(ti => ti.TestTaskInstance.All(tti => tti.Checked));


            // pretpostavljajuci da 1 task nosi 1 bod, ukupno se može sakupit onoliko bodova koliko ima taskova u testu
            var totalPoints = testInstances.First().TestTaskInstance.Count();




            // test instanca je cijela checked ako su svi taskovi u njoj checked (znaci teacher je ocjenio sve non map taskove)
            var checkedInstances = testInstances.Where(ti => ti.TestTaskInstance.All(tti => tti.Checked)).ToList();


            var correctTasksCount = checkedInstances.Sum(ti => ti.TestTaskInstance.Count(tti => tti.Correct));
            var checkedInstancesCount = checkedInstances.Count;

            var avgPoints = checkedInstancesCount > 0 ? (decimal)correctTasksCount / checkedInstancesCount : 0;


            var testInstancesDto = await _context.TestInstance
                .Where(ti => ti.TestInstanceBaseId == testInstanceBaseId)
                .Select(ti => new TestInstanceForTeacherDto
                {
                    Id = ti.Id,
                    Student = ti.Student.FirstName + " " + ti.Student.LastName,
                    ElapsedTime = ti.ElapsedTime ?? TimeSpan.Zero,
                    Points = ti.TestTaskInstance.Count(tti => tti.Correct),
                    Started = ti.Started,
                    Finished = ti.Finished,
                    Checked = ti.TestTaskInstance.All(tti => tti.Checked)
                })
                .ToListAsync();

            return new TestPublishedDetailsDto
            {
                Id = testInstanceBase.Test.Id,
                Duration = testInstanceBase.Test.Duration,
                Name = testInstanceBase.Test.Name,
                Description = testInstanceBase.Test.Description,
                Subject = testInstanceBase.Test.Subject.Name,
                FinishedByStudentsInstanceCount = finishedByStudentsInstanceCount,
                FinishedInstanceCount = finishedInstanceCount,
                InstanceCount = instanceCount,
                Active = active,
                CheckedInstanceCount = checkedInstanceCount,
                AvgElapsedTime = avgElapsedTime,
                TotalPoints = totalPoints,
                AvgPoints = avgPoints,
                TestInstances = testInstancesDto
            };
        }

        public async Task<int> UpdateTest(CreateTestDto test, int id)
        {
            var _test = await _context.Test.FirstOrDefaultAsync(t => t.Id == test.Id && t.TeacherId == id);


            if (_test is null)
            {
                throw new Exception($"Test with id = {test.Id} does not exist or does not belong to teacher with id = {id}");
            }

            _test.Name = test.Name;
            _test.Description = test.Description;
            _test.SubjectId = test.SubjectId;
            _test.Duration = test.Duration;

            await _context.SaveChangesAsync();

            return _test.Id;
        }

        public async Task<Test> GetTestByTestInstanceId(int testInstanceId, int studentId)
        {
            var _testInstance = await _context.TestInstance
                .Include(ti => ti.TestInstanceBase)
                .Include(ti => ti.TestInstanceBase.Test)
                .FirstOrDefaultAsync(ti => ti.Id == testInstanceId && ti.StudentId == studentId);

            if (_testInstance is null)
            {
                throw new Exception($"Problems getting data");
            }
            else
            {
                var _test = await _context.Test
                       .Include(t => t.Subject)
                       .Include(t => t.Subject.Student)
                       .Include(t => t.TestTask)
                       .FirstOrDefaultAsync(t => t.Id == _testInstance.TestInstanceBase.TestId);



                if (_test is null)
                {
                    throw new Exception($"Problems getting data");
                }
                else
                {
                    return _test;
                }


            }
        }
    }
}
