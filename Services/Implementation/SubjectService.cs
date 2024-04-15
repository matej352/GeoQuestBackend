using GeoQuest.DTOs;
using GeoQuest.DTOs.Extensions;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Repositories;
using GeoQuest.Utils;

namespace GeoQuest.Services.Implementation
{
    public class SubjectService : ISubjectService
    {

        private readonly UserContext _userContext;

        private readonly ISubjectRepository _subjectRepository;


        public SubjectService(UserContext userContext, ISubjectRepository subjectRepository)
        {
            _userContext = userContext;
            _subjectRepository = subjectRepository;
        }

        public async Task AddStudents(int subjectId, List<int> studentIds)
        {
            await _subjectRepository.AddStudents(subjectId, studentIds);
        }

        public async Task<SubjectDto> CreateSubject(SubjectDto subject)
        {
            var subjectId = await _subjectRepository.SaveSubject(subject, _userContext.Id);

            var _subject = await _subjectRepository.GetSubject(subjectId);

            return _subject.AsSubjectDto();
        }

        public async Task<SubjectDetailsDto> GetSubjectDetails(int subjectId)
        {
            var subject = await _subjectRepository.GetSubjectDetails(subjectId);

            return subject;
        }

        public async Task<IEnumerable<SubjectDto>> GetSubjects(int teacherId)
        {
            var subjects = await _subjectRepository.GetSubjects(teacherId);

            List<SubjectDto> subjectDtoList = subjects.Select(s => s.AsSubjectDto()).ToList();

            return subjectDtoList;
        }
    }
}
