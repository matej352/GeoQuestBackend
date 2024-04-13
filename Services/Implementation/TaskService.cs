using GeoQuest.DTOs;
using GeoQuest.DTOs.Extensions;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Repositories;
using GeoQuest.Repositories.Implementation;

namespace GeoQuest.Services.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly UserContext _userContext;

        private readonly ITaskRepository _taskRepository;


        public TaskService(UserContext userContext, ITaskRepository taskRepository)
        {
            _userContext = userContext;
            _taskRepository = taskRepository;
        }


        public async Task<TaskDto> CreateTask(TaskDto task)
        {
            var taskId = await _taskRepository.SaveTask(task, _userContext.Id);

            var _task = await _taskRepository.GetTask(taskId);

            return _task.AsTaskDto();
        }

        public async Task<IEnumerable<TaskDto>> GetTasks(int testId)
        {
            var tasks = await _taskRepository.GetTasks(testId);

            List<TaskDto> taskDtoList = tasks.Select(t => t.AsTaskDto()).ToList();

            return taskDtoList;
        }

    }
}
