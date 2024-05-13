using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface ITaskService
    {
        public Task<TaskDto> CreateTask(TaskDto task);
        public Task DeleteTask(int taskId);
        public Task<IEnumerable<TaskDto>> GetTasks(int testId);
    }
}
