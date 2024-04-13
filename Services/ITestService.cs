using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface ITestService
    {
        public Task<TestDto> CreateTest(CreateTestDto test);

        public Task<IEnumerable<TestDto>> GetTests(int teacherId);
    }
}
