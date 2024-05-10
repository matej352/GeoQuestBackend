using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Services
{
    public interface ISubjectService
    {
        public Task<IEnumerable<SubjectDto>> GetSubjects(int teacherId);

        public Task<SubjectDetailsDto> GetSubjectDetails(int subjectId);

        public Task<SubjectDto> CreateSubject(SubjectDto subject);

        public Task AddStudents(int subjectId, List<int> studentIds);

        public Task<SubjectDto> UpdateSubject(UpdateSubjectDto subjectDto);
    }
}
