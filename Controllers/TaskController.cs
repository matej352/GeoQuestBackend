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
        public async Task<ActionResult<TaskDto>> CreateTask(TaskDto taskDto)
        {

            var newTask = await _taskService.CreateTask(taskDto);

            return Ok(newTask);

        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult<TaskDto>> DeleteTask(int taskId)
        {

            await _taskService.DeleteTask(taskId);

            return Ok();

        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Tasks")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> Tasks(int testId)
        {

            var tasks = await _taskService.GetTasks(testId);

            return Ok(tasks);

        }



    }
}
