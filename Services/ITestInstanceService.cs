using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface ITestInstanceService
    {

        //public Task<TestInstanceDto> GetOnGoingTestInstance(int instanceId);
        public Task<IEnumerable<TestInstanceDto>> GetTestInstances(int studentId, bool finished = false);
        public Task<TestInstanceDetailsDto> GetTestInstance(int testInstanceId);
        public Task StartTestInstance(int instanceId);
        public Task FinishTestInstance(int instanceId, TimeSpan elapsedTime);
        public Task UpdateElapsedTime(int instanceId, TimeSpan elapsedTime);
        public Task<TestInstanceResultDto> GetTestInstanceResult(int testInstanceId);
    }
}
