using GeoQuest.DTOs;
using GeoQuest.DTOs.Extensions;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Models;
using GeoQuest.Repositories;
using GeoQuest.Repositories.Implementation;

namespace GeoQuest.Services.Implementation
{
    public class TestService : ITestService
    {
        private readonly UserContext _userContext;

        private readonly ITestRepository _testRepository;


        public TestService(UserContext userContext, ITestRepository testRepository)
        {
            _userContext = userContext;
            _testRepository = testRepository;
        }




        public async Task<TestDto> CreateTest(CreateTestDto test)
        {
            var testId = await _testRepository.SaveTest(test, _userContext.Id);

            var _test = await _testRepository.GetTest(testId);

            return _test.AsTestDto();
        }

        public async Task<TestDto> GetTest(int testId)
        {
            var test = await _testRepository.GetTest(testId);

            var testDto = test.AsTestDto();

            return testDto;
        }

        public async Task<IEnumerable<TestDto>> GetTests(int teacherId)
        {
            var tests = await _testRepository.GetTests(teacherId);

            List<TestDto> testDtoList = tests.Select(t => t.AsTestDto()).ToList();

            return testDtoList;
        }

        public async Task PublishTest(int testId)
        {
            await _testRepository.PublishTest(testId);
        }
    }
}
