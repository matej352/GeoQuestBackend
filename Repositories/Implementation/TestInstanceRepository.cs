using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoQuest.Repositories.Implementation
{
    public class TestInstanceRepository : ITestInstanceRepository
    {

        private readonly GeoQuestContext _context;

        public TestInstanceRepository(GeoQuestContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<TestInstance>> GetTestInstances(int studentId)
        {
            var testInstances = await _context.TestInstance
                .Include(t => t.TestInstanceBase.Test)
                .Include(t => t.TestInstanceBase.Test.Teacher)
                .Include(t => t.TestInstanceBase.Test.Subject)
                .Where(t => t.StudentId == studentId && t.Started == false && t.Finished == false && t.TestInstanceBase.Active == true).ToListAsync();       // dohvati samo one instance koje jos nisu pokrenutei koje nisu rijesene i koje se jos uvijek mogu pokrenuti (učitelj nije zatvorio ispit)


            return testInstances;
        }

        public async Task StartTestInstance(int instanceId, int studentId)
        {
            var instance = await _context.TestInstance.Where(t => t.StudentId == studentId && t.Id == instanceId).FirstOrDefaultAsync();

            if (instance == null)
            {
                throw new Exception($"Test instance with id = {instanceId} does not exist for student with id = {studentId}");
            }

            instance.Started = true;
            await _context.SaveChangesAsync();
        }

        public async Task FinishTestInstance(int instanceId, int studentId)
        {
            var instance = await _context.TestInstance.Where(t => t.StudentId == studentId && t.Id == instanceId).FirstOrDefaultAsync();

            if (instance == null)
            {
                throw new Exception($"Test instance with id = {instanceId} does not exist for student with id = {studentId}");
            }

            instance.Finished = true;
            await _context.SaveChangesAsync();
        }
    }
}
