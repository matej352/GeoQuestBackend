using GeoQuest.DTOs;
using GeoQuest.DTOs.Extensions;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Repositories;

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




        public async Task<TestDto> CreateTest(TestDto test)
        {
            var testId = await _testRepository.SaveTest(test, _userContext.Id);

            var _test = await _testRepository.GetTest(testId);

            return _test.AsTestDto();
        }
    }
}
