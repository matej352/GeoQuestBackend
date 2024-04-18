using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface ITestInstanceService
    {

        //public Task<TestInstanceDto> GetOnGoingTestInstance(int instanceId);
        public Task<IEnumerable<TestInstanceDto>> GetTestInstances(int studentId);
        public Task StartTestInstance(int instanceId);
        public Task FinishTestInstance(int instanceId);
    }
}
