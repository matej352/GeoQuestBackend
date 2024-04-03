using GeoQuest.DTOs;
using GeoQuest.Models;
using Microsoft.Extensions.Logging;

namespace GeoQuest.Repositories
{
    public interface ISubjectRepository
    {
        public Task<IEnumerable<Subject>> GetSubjects(int teacherId);

        public Task<int> SaveSubject(SubjectDto subject, int teacherId);

        public Task<Subject> GetSubject(int id);
    }
}
