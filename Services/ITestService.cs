using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface ITestService
    {
        public Task<TestDto> CreateTest(CreateTestDto test);

        public Task PublishTest(int testId);

        public Task CloseTest(int testInstanceBaseId);

        public Task<IEnumerable<TestDto>> GetTests(int teacherId);

        public Task<TestDto> GetTest(int testId);

        public Task<IEnumerable<TestPublishedDto>> GetPublishedTests(int teacherId);
        public Task<TestPublishedDetailsDto> GetPublishedTestOverview(int testInstanceBaseId);
        public Task<TestDto> UpdateTest(CreateTestDto test);
    }
}
