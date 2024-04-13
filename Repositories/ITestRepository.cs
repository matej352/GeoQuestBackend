using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Repositories
{
    public interface ITestRepository
    {
        public Task<IEnumerable<Test>> GetTests(int teacherId);

        public Task<int> SaveTest(CreateTestDto test, int teacherId);

        public Task<Test> GetTest(int id);

    }
}
