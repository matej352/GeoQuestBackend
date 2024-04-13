using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Repositories
{
    public interface ITaskRepository
    {

        public Task<int> SaveTask(TaskDto task, int testId);

        public Task<Models.Task> GetTask(int id);

        public Task<IEnumerable<Models.Task>> GetTasks(int testId);
    }
}
