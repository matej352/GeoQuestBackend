using GeoQuest.DTOs;
using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoQuest.Repositories.Implementation
{
    public class TestRepository : ITestRepository
    {

        private readonly GeoQuestContext _context;

        public TestRepository(GeoQuestContext context)
        {
            _context = context;
        }


        public async Task<Test> GetTest(int id)
        {
            var _test = await _context.Test.FirstOrDefaultAsync(t => t.Id == id);

            if (_test is null)
            {
                throw new Exception($"Test with id = {id} does not exists");
            }
            else
            {
                return _test;
            }
        }

        public async Task<IEnumerable<Test>> GetTests(int teacherId)
        {
            var tests = await _context.Test.Include(t => t.Subject).Where(t => t.TeacherId == teacherId).ToListAsync();
            return tests;
        }


        public async Task<int> SaveTest(CreateTestDto test, int teacherId)
        {
            Test newTest = new Test
            {
                Duration = test.Duration,
                Description = test.Description,
                SubjectId = test.SubjectId,
                TeacherId = teacherId,
            };

            _context.Add(newTest);
            await _context.SaveChangesAsync();

            return newTest.Id;
        }



    }
}
