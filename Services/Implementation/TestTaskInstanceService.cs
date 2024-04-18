using GeoQuest.DTOs;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Models;
using GeoQuest.Repositories;

namespace GeoQuest.Services.Implementation
{
    public class TestTaskInstanceService : ITestTaskInstanceService
    {

        private readonly UserContext _userContext;

        private readonly ITestTaskInstanceRepository _testTaskInstanceRepository;


        public TestTaskInstanceService(UserContext userContext, ITestTaskInstanceRepository testTaskInstanceRepository)
        {
            _userContext = userContext;
            _testTaskInstanceRepository = testTaskInstanceRepository;
        }






        public async Task<IEnumerable<TaskInstanceDto>> GetOnGoingTestTaskInstances(int testInstanceId)
        {
            return await _testTaskInstanceRepository.GetOnGoingTestTaskInstances(testInstanceId, _userContext.Id);
        }

        public async Task SaveOnGoingTestTaskInstanceAnswer(TestTaskInstanceAnswerSaveDto saveAnswer)
        {
            await _testTaskInstanceRepository.SaveOnGoingTestTaskInstanceAnswer(saveAnswer, _userContext.Id);
        }
    }
}
