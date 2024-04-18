using GeoQuest.Models;

namespace GeoQuest.Repositories
{
    public interface ITestInstanceRepository
    {

        public Task<IEnumerable<TestInstance>> GetTestInstances(int studentId);
        public Task StartTestInstance(int instanceId, int studentId);
        public Task FinishTestInstance(int instanceId, int studentId);
    }
}
