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
        public async Task<ActionResult<TestDto>> CreateTest(CreateTestDto testDto)
        {

            var newTest = await _testService.CreateTest(testDto);

            return Ok(newTest);

        }


        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Tests")]
        public async Task<ActionResult<IEnumerable<TestDto>>> Tests()
        {

            var tests = await _testService.GetTests(_userContext.Id);

            return Ok(tests);

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
