using GeoQuest.DTOs;
using GeoQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoQuest.Repositories.Implementation
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly GeoQuestContext _context;

        public SubjectRepository(GeoQuestContext context)
        {
            _context = context;
        }

        public async Task<Subject> GetSubject(int id)
        {
            var _subject = await _context.Subject.FirstOrDefaultAsync(e => e.Id == id);

            if (_subject is null)
            {
                throw new Exception($"Subject with id = {id} does not exists");
            }
            else
            {
                return _subject;
            }
        }

        public async Task<IEnumerable<Subject>> GetSubjects(int teacherId)
        {
            var subjects = await _context.Subject.Where(subject => subject.TeacherId == teacherId).ToListAsync();
            return subjects;
        }

        public async Task<int> SaveSubject(SubjectDto subject, int teacherId)
        {
            Subject newSubject = new Subject
            {
                Name = subject.Name,
                TeacherId = teacherId,
            };

            _context.Add(newSubject);
            await _context.SaveChangesAsync();

            return newSubject.Id;
        }
    }
}
