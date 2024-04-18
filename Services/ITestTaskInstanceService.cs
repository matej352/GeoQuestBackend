using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface ITestTaskInstanceService
    {
        public Task<IEnumerable<TaskInstanceDto>> GetOnGoingTestTaskInstances(int testInstanceId);
        public Task SaveOnGoingTestTaskInstanceAnswer(TestTaskInstanceAnswerSaveDto saveAnswer);
    }
}
