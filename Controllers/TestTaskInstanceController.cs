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
    public class TestTaskInstanceController : ControllerBase
    {


        private readonly UserContext _userContext;
        private readonly ITestTaskInstanceService _testTaskInstanceService;

        public TestTaskInstanceController(
            ITestTaskInstanceService testTaskInstanceService,
            UserContext userContext)
        {
            _testTaskInstanceService = testTaskInstanceService;
            _userContext = userContext;
        }



        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("OnGoingTestTaskInstances")]
        public async Task<ActionResult<TaskInstanceDto>> GetOnGoingTestTaskInstances(int testInstanceId)
        {

            var test = await _testTaskInstanceService.GetOnGoingTestTaskInstances(testInstanceId);

            return Ok(test);

        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [Route("SaveAnswer")]
        public async Task<ActionResult> SaveOnGoingTestTaskInstanceAnswer(TestTaskInstanceAnswerSaveDto saveAnswer)
        {

            await _testTaskInstanceService.SaveOnGoingTestTaskInstanceAnswer(saveAnswer);

            return Ok();

        }


    }
}
