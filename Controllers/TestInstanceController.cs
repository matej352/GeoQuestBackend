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
        [HttpPost]
        [Route("Start")]
        public async Task<ActionResult> StartTestInstance(int instanceId)
        {

            await _testInstanceService.StartTestInstance(instanceId);

            return Ok();

        }


        [Authorize(Roles = "Student")]
        [HttpPost]
        [Route("Finish")]
        public async Task<ActionResult> FinishTestInstance(int instanceId)
        {

            await _testInstanceService.FinishTestInstance(instanceId);

            return Ok();

        }




    }
}
