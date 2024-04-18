using GeoQuest.DTOs;
using GeoQuest.DTOs.Extensions;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Repositories;

namespace GeoQuest.Services.Implementation
{
    public class TestInstanceService : ITestInstanceService
    {
        private readonly UserContext _userContext;

        private readonly ITestInstanceRepository _testInstanceRepository;


        public TestInstanceService(UserContext userContext, ITestInstanceRepository testInstanceRepository)
        {
            _userContext = userContext;
            _testInstanceRepository = testInstanceRepository;
        }


        public async Task<IEnumerable<TestInstanceDto>> GetTestInstances(int studentId)
        {
            var instances = await _testInstanceRepository.GetTestInstances(studentId);

            var dtos = instances.Select(t => t.AsTestInstanceDto()).ToList();

            return dtos;
        }

        public async Task StartTestInstance(int instanceId)
        {
            await _testInstanceRepository.StartTestInstance(instanceId, _userContext.Id);
        }


        public async Task FinishTestInstance(int instanceId)
        {
            await _testInstanceRepository.FinishTestInstance(instanceId, _userContext.Id);
        }
    }
}
