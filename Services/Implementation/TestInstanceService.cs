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


        public async Task<IEnumerable<TestInstanceDto>> GetTestInstances(int studentId, bool finished)
        {
            var instances = await _testInstanceRepository.GetTestInstances(studentId, finished);

            var dtos = instances.Select(t => t.AsTestInstanceDto()).ToList();

            return dtos;
        }

        public async Task StartTestInstance(int instanceId)
        {
            await _testInstanceRepository.StartTestInstance(instanceId, _userContext.Id);
        }


        public async Task FinishTestInstance(int instanceId, TimeSpan elapsedTime)
        {
            await _testInstanceRepository.FinishTestInstance(instanceId, elapsedTime, _userContext.Id);

            await _testInstanceRepository.AutoGradeTestInstance(instanceId);

        }

        public async Task<TestInstanceDetailsDto> GetTestInstance(int testInstanceId)
        {
            var instance = await _testInstanceRepository.GetTestInstance(testInstanceId);

            var dto = instance.AsTestInstanceDetailsDto();

            return dto;
        }

        public async Task UpdateElapsedTime(int instanceId, TimeSpan elapsedTime)
        {
            await _testInstanceRepository.UpdateElapsedTime(instanceId, elapsedTime, _userContext.Id);
        }

        public async Task<TestInstanceResultDto> GetTestInstanceResult(int testInstanceId)
        {
            //provjeri je li user admin ili ako je student da gleda svoj ipit, a ne nečiji tuđi

            var result = await _testInstanceRepository.GetTestInstanceResult(testInstanceId);

            return result;


        }
    }
}
