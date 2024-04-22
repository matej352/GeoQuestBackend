using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Repositories
{
    public interface ITestInstanceRepository
    {

        public Task<IEnumerable<TestInstance>> GetTestInstances(int studentId, bool finished = false);
        public Task<TestInstance> GetTestInstance(int testInstanceId);
        public Task StartTestInstance(int instanceId, int studentId);
        public Task FinishTestInstance(int instanceId, TimeSpan elapsedTime, int studentId);
        public Task UpdateElapsedTime(int instanceId, TimeSpan elapsedTime, int studentId);
        public Task<TestInstanceResultDto> GetTestInstanceResult(int testInstanceId);
    }
}
