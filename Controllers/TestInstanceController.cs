using GeoQuest.DTOs;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Services;
using GeoQuest.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoQuest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestInstanceController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly ITestInstanceService _testInstanceService;

        public TestInstanceController(
            ITestInstanceService testInstanceService,
            UserContext userContext)
        {
            _testInstanceService = testInstanceService;
            _userContext = userContext;
        }


        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("TestInstances")]
        public async Task<ActionResult<IEnumerable<TestInstanceDto>>> TestInstances()
        {

            var tests = await _testInstanceService.GetTestInstances(_userContext.Id);

            return Ok(tests);

        }


        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("TestInstances/Previous")]
        public async Task<ActionResult<IEnumerable<TestInstanceDto>>> PreviousTestInstances()
        {

            var tests = await _testInstanceService.GetTestInstances(_userContext.Id, true);

            return Ok(tests);

        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("TestInstance")]
        public async Task<ActionResult<TestInstanceDetailsDto>> TestInstance(int testInstanceId)
        {

            var test = await _testInstanceService.GetTestInstance(testInstanceId);

            return Ok(test);

        }

        [Authorize]
        [HttpGet]
        [Route("TestInstance/Result")]
        public async Task<ActionResult<TestInstanceResultDto>> GetTestInstanceResult(int testInstanceId)
        {

            var result = await _testInstanceService.GetTestInstanceResult(testInstanceId);

            return Ok(result);

        }


        [Authorize(Roles = "Student")]
        [HttpPost]
        [Route("Start")]
        public async Task<ActionResult> StartTestInstance([FromBody] int instanceId)
        {

            await _testInstanceService.StartTestInstance(instanceId);

            return Ok();

        }


        [Authorize(Roles = "Student")]
        [HttpPost]
        [Route("ElapsedTime")]
        public async Task<ActionResult> UpdateElapsedTime(UpdateElapsedTimeDto updateElapsedTimeDto)
        {

            await _testInstanceService.UpdateElapsedTime(updateElapsedTimeDto.Id, updateElapsedTimeDto.ElapsedTime);

            return Ok();

        }


        [Authorize(Roles = "Student")]
        [HttpPost]
        [Route("Finish")]
        public async Task<ActionResult> FinishTestInstance(FinishTestInstanceDto finishTestInstanceDto)
        {

            await _testInstanceService.FinishTestInstance(finishTestInstanceDto.Id, finishTestInstanceDto.ElapsedTime);

            return Ok();

        }




    }
}
