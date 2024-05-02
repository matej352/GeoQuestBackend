using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Repositories
{
    public interface ITestTaskInstanceRepository
    {
        public Task<IEnumerable<TaskInstanceDto>> GetOnGoingTestTaskInstances(int testInstanceId, int studentId);
        public Task GradeTestTaskInstance(GradeTestTaskInstanceDto grade, int teacherId);
        public Task SaveOnGoingTestTaskInstanceAnswer(TestTaskInstanceAnswerSaveDto saveAnswer, int studentId);
    }
}
