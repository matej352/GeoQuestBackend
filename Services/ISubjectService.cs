using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Services
{
    public interface ISubjectService
    {
        public Task<IEnumerable<SubjectDto>> GetSubjects(int teacherId);

        public Task<SubjectDto> CreateSubject(SubjectDto subject);
    }
}
