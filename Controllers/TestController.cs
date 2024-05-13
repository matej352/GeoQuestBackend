using GeoQuest.DTOs;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Services;
using GeoQuest.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace GeoQuest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly UserContext _userContext;
        private readonly ITestService _testService;

        public TestController(
            ITestService testService,
            UserContext userContext)
        {
            _testService = testService;
            _userContext = userContext;
        }





        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateTest(CreateTestDto testDto)
        {

            var newTest = await _testService.CreateTest(testDto);

            return Ok(newTest);

        }


        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("Publish")]
        public async Task<ActionResult<int>> PublishTest([FromBody] int testId)
        {

            var testInstanceBaseId = await _testService.PublishTest(testId);

            return Ok(testInstanceBaseId);

        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("Close")]
        public async Task<ActionResult<TestDto>> CloseTest([FromBody] int testInstanceBaseId)
        {

            await _testService.CloseTest(testInstanceBaseId);

            return Ok();

        }



        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Tests")]
        public async Task<ActionResult<IEnumerable<TestDto>>> Tests()
        {

            var tests = await _testService.GetTests(_userContext.Id);

            return Ok(tests);

        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Tests/Published")]
        public async Task<ActionResult<IEnumerable<TestPublishedDto>>> PublishedTests()
        {

            var tests = await _testService.GetPublishedTests(_userContext.Id);

            return Ok(tests);

        }


        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Test")]
        public async Task<ActionResult<TestDto>> Test(int testId)
        {

            var test = await _testService.GetTest(testId);

            return Ok(test);

        }

        [Authorize(Roles = "Teacher")]
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<TestDto>> UpdateTest(CreateTestDto test)
        {

            var _test = await _testService.UpdateTest(test);

            return Ok(_test);

        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Test/Published/Overview")]
        public async Task<ActionResult<TestPublishedDetailsDto>> PublishedTestOverview(int testInstanceBaseId)
        {

            var test = await _testService.GetPublishedTestOverview(testInstanceBaseId);

            return Ok(test);

        }

        /*// PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
         
                 // GET: api/<TestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
         
         */
    }
}
