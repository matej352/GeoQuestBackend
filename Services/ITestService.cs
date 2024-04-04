using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface ITestService
    {
        public Task<TestDto> CreateTest(TestDto test);
    }
}
