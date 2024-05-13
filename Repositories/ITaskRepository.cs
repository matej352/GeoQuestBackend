using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Repositories
{
    public interface ITaskRepository
    {

        public Task<int> SaveTask(TaskDto task, int testId);

        public Task<TestTask> GetTask(int id);

        public Task<IEnumerable<TestTask>> GetTasks(int testId);
        public Task DeleteTask(int taskId, int userId);
    }
}
