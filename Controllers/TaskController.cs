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
    public class TaskController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly ITaskService _taskService;

        public TaskController(
            ITaskService taskService,
            UserContext userContext)
        {
            _taskService = taskService;
            _userContext = userContext;

        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<SubjectDto>> CreateTask(TaskDto taskDto)
        {

            var newSubject = await _taskService.CreateTask(taskDto);

            return Ok(newSubject);

        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Tasks")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> Tasks(int testId)
        {

            var tasks = await _taskService.GetTasks(_userContext.Id);

            return Ok(tasks);

        }



    }
}
