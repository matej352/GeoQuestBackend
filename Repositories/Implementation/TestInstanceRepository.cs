using GeoQuest.DTOs;
using GeoQuest.Enums;
using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.OverlayNG;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

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


            var testInstance = await _context.TestInstance.Where(ti => ti.Id == testInstanceId && ti.Finished == true).FirstOrDefaultAsync();
            if (testInstance == null)
            {
                throw new Exception($"TestInstance with id={testInstanceId} has not been finished with solving");
            }


            var testInstanceResultDto = await _context.TestInstance
                                            .Include(ti => ti.Student)
                                            .Include(ti => ti.TestInstanceBase)
                                            .Include(ti => ti.TestInstanceBase.Test)
                                            .Where(ti => ti.Id == testInstanceId)
                                            .Select(ti => new TestInstanceResultDto
                                            {
                                                TestInstanceId = ti.Id,
                                                TestName = ti.TestInstanceBase.Test.Name,
                                                Student = ti.Student.FirstName + " " + ti.Student.LastName,
                                                AllChecked = ti.TestTaskInstance.All(tti => tti.Checked) ? true : false,
                                                StudentTotalPoints = ti.TestTaskInstance.Count(tt => tt.Correct),    // Each correct task is 1 point,
                                                TestTotalPoints = ti.TestTaskInstance.Count(),    // Each correct task is 1 point
                                                SuccessPercentage = (double)ti.TestTaskInstance.Count(tt => tt.Checked && tt.Correct) / ti.TestTaskInstance.Count(tt => tt.Checked) * 100,
                                                TestTasks = ti.TestTaskInstance.Select(tti => new TestTaskResultDto
                                                {
                                                    Id = tti.Id,
                                                    MapCenter = tti.TestTask.MapCenter,
                                                    MapType = (MapType?)tti.TestTask.MapType,   //vjv ne treba ? i u bazi mogu ovi podaci za Map biti not null, trenutno su nullable
                                                    MapZoomLevel = tti.TestTask.MapZoomLevel,
                                                    Type = (TaskType)tti.TestTask.Type,
                                                    Checked = tti.Checked,
                                                    Question = tti.TestTask.Question,
                                                    NonMapPoint = tti.TestTask.NonMapPoint,
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

        public async Task AutoGradeTestInstance(int testInstanceId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var testTaskInstances = await _context.TestTaskInstance
                                                        .Where(t => t.TestInstanceId == testInstanceId)
                                                        .ToListAsync();

                    foreach (var testTaskInstance in testTaskInstances)
                    {
                        await ProcessTaskInstance(testTaskInstance);
                    }



                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception($"Problem while auto grading test instance with ID {testInstanceId}! " + exception.Message);
                }
            }
        }

        private async Task ProcessTaskInstance(TestTaskInstance testTaskInstance)
        {

            var testTask = await _context.TestTask
                               .Include(t => t.Options)
                               .Include(t => t.Options.OptionAnswer)
                               .FirstOrDefaultAsync(t => t.Id == testTaskInstance.TestTaskId);

            if (testTask is null)
            {
                throw new Exception($"Problem while fetching task based on task instace id {testTaskInstance.TestTaskId}!");
            }


            switch ((TaskType)testTaskInstance.TestTask.Type)
            {
                case TaskType.MarkPoint:
                    await ProcessMarkPointTask(testTask, testTaskInstance);
                    break;
                case TaskType.MarkPolygon:
                    await ProcessMarkPolygonTask(testTask, testTaskInstance);
                    break;
                case TaskType.SelectPoint:
                    await ProcessSelectedOption(testTask, testTaskInstance);
                    break;
                case TaskType.SelectPolygon:
                    await ProcessSelectedOption(testTask, testTaskInstance);
                    break;
                case TaskType.NonMap:
                    // Implement logic for NonMap task type
                    // Non map task needs to be evaluated manually
                    break;
                default:
                    throw new Exception("Unsupported test task type");
            }
        }

        private async Task ProcessMarkPointTask(TestTask testTask, TestTaskInstance testTaskInstance)
        {

            if (testTaskInstance.StudentAnswer == null)
            {
                testTaskInstance.Correct = false;
                testTaskInstance.Checked = true;

                await _context.SaveChangesAsync();

                return;
            }

            // Parse the correct answer point from TestTask.Answer
            var correctAnswerParts = testTask.Answer.Split(',');
            var correctAnswerLat = double.Parse(correctAnswerParts[0]);
            var correctAnswerLng = double.Parse(correctAnswerParts[1]);
            var correctAnswerPoint = new Point(correctAnswerLng, correctAnswerLat) { SRID = 4326 };

            // Parse the student's answer point from TestTaskInstance.StudentAnswer
            var studentAnswerParts = testTaskInstance.StudentAnswer.Split(',');
            var studentAnswerLat = double.Parse(studentAnswerParts[0]);
            var studentAnswerLng = double.Parse(studentAnswerParts[1]);
            var studentAnswerPoint = new Point(studentAnswerLng, studentAnswerLat) { SRID = 4326 };

            // Calculate the distance between the correct answer point and the student's answer point
            var distance = correctAnswerPoint.Distance(studentAnswerPoint);

            // Define the maximum allowed distance (radius) around the correct answer point
            var maxDistanceInDegrees = 0.3; // For example, 0.3 degrees --> 111,000 meters per degree, so 0.3 * 111km = 33.3km   (10km, 30km, 50km, 100km)



            // Check if the student's answer is within the maximum allowed distance from the correct answer point  --> TODO: ISTESTIRATI!!!!
            if (distance <= maxDistanceInDegrees)
            {
                testTaskInstance.Correct = true;
            }
            else
            {
                testTaskInstance.Correct = false;
            }
            testTaskInstance.Checked = true;

            await _context.SaveChangesAsync();
        }

        private async Task ProcessMarkPolygonTask(TestTask testTask, TestTaskInstance testTaskInstance)
        {

            if (testTaskInstance.StudentAnswer == null)
            {
                testTaskInstance.Correct = false;
                testTaskInstance.Checked = true;

                await _context.SaveChangesAsync();

                return;
            }


            // Parse the JSON strings into arrays of points
            List<Coordinate> answerCoords = ParseCoordinates(testTask.Answer);
            List<Coordinate> studentAnswerCoords = ParseCoordinates(testTaskInstance.StudentAnswer);

            // Ensure closed linestring for answerCoords
            if (answerCoords.First() != answerCoords.Last())
            {
                // Add the first coordinate to the end to close the linestring
                answerCoords.Add(answerCoords.First());
            }

            // Ensure closed linestring for studentAnswerCoords
            if (studentAnswerCoords.First() != studentAnswerCoords.Last())
            {
                // Add the first coordinate to the end to close the linestring
                studentAnswerCoords.Add(studentAnswerCoords.First());
            }

            // Create Polygon objects from the arrays of points
            Polygon answerPolygon = new Polygon(new LinearRing(answerCoords.ToArray()));
            Polygon studentAnswerPolygon = new Polygon(new LinearRing(studentAnswerCoords.ToArray()));

            // Calculate the intersection of the two polygons
            Geometry intersection = OverlayNG.Overlay(answerPolygon, studentAnswerPolygon, OverlayNG.INTERSECTION);

            // Calculate the area of the intersection polygon
            double intersectionArea = intersection.Area;

            // Calculate the area of each input polygon
            double answerArea = answerPolygon.Area;
            double studentAnswerArea = studentAnswerPolygon.Area;

            // Determine the minimum area of the input polygons
            double minArea = Math.Min(answerArea, studentAnswerArea);

            // Calculate the percentage of overlap
            double overlapPercentage = (intersectionArea / minArea) * 100;  // between 0 and 100

            if (overlapPercentage >= 70)
            {
                testTaskInstance.Correct = true;
            }
            else
            {
                testTaskInstance.Correct = false;
            }
            testTaskInstance.Checked = true;

            await _context.SaveChangesAsync();
        }

        private async Task ProcessSelectedOption(TestTask testTask, TestTaskInstance testTaskInstance)
        {

            if (testTaskInstance.StudentAnswer == null)
            {
                testTaskInstance.Correct = false;
                testTaskInstance.Checked = true;

                await _context.SaveChangesAsync();

                return;
            }

            // Get the correct answer options --> FOR NOW, THERE CAN BE ONLY 1 CORRECT OPTION
            List<OptionAnswer> correctAnswers = testTask.Options.OptionAnswer.Where(a => a.Correct).ToList();

            // Parse the student's answer
            int selectedOptionId;

            if (!int.TryParse(testTaskInstance.StudentAnswer, out selectedOptionId))
            {
                throw new ArgumentException("Invalid student answer on task of type SelectPoint/SelectPolygon. Answer must be the ID of an option answer.");
            }

            // Check if the selected option is correct
            bool isCorrect = correctAnswers.Any(a => a.Id == selectedOptionId);

            if (isCorrect)
            {
                testTaskInstance.Correct = true;
            }
            else
            {
                testTaskInstance.Correct = false;
            }
            testTaskInstance.Checked = true;

            await _context.SaveChangesAsync();
        }


        private List<Coordinate> ParseCoordinates(string coordinatesJson)
        {
            // Parse the JSON string and extract latitudes and longitudes
            // Assuming the JSON structure is an array of objects with "lat" and "lng" properties
            List<Coordinate> coordinates = new List<Coordinate>();

            try
            {
                dynamic[]? points = JsonConvert.DeserializeObject<dynamic[]>(coordinatesJson);

                if (points != null)
                {
                    foreach (var point in points)
                    {
                        double lat = point.lat;
                        double lng = point.lng;
                        coordinates.Add(new Coordinate(lat, lng));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any parsing errors
                throw new Exception("Error parsing answer or studentAnswer coordinates JSON: " + ex.Message);
            }

            return coordinates;
        }
    }
}
