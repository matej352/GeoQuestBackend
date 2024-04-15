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

        public async Task AddStudents(int subjectId, List<int> studentIds)
        {

            var subject = await _context.Subject.FindAsync(subjectId);

            if (subject != null)
            {
                // Fetch students by their IDs
                var studentsToAdd = await _context.Account.Where(student => studentIds.Contains(student.Id)).ToListAsync();

                // Add students to the subject's Student collection
                foreach (var student in studentsToAdd)
                {
                    subject.Student.Add(student);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Subject with id = {subjectId} does not exists");
            }
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

        public async Task<SubjectDetailsDto> GetSubjectDetails(int subjectId)
        {
            var subjectDetails = await _context.Subject
            .Where(s => s.Id == subjectId)
            .Select(s => new SubjectDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Students = s.Student.Select(st => new StudentDto
                {
                    Id = st.Id,
                    FirstName = st.FirstName,
                    LastName = st.LastName,
                    Email = st.Email,
                }).ToList(),
                TestInstancesBase = s.Test
                    .SelectMany(t => t.TestInstanceBase)
                    .Select(ti => new TestInstanceBaseDto
                    {
                        Id = ti.Id,
                        TestName = ti.Test.Name,
                        InstancesCount = ti.InstancesCount,
                        Active = ti.Active
                    }).ToList()
            })
            .FirstOrDefaultAsync();

            if (subjectDetails is null)
            {
                throw new Exception($"Subject with id = {subjectId} does not exists");
            }

            return subjectDetails;
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
                Description = subject.Description,
                TeacherId = teacherId,
            };

            _context.Add(newSubject);
            await _context.SaveChangesAsync();

            return newSubject.Id;
        }
    }
}
