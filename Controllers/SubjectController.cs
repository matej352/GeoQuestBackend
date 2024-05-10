using GeoQuest.DTOs;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoQuest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {

        private readonly UserContext _userContext;
        private readonly ISubjectService _subjectService;

        public SubjectController(
            ISubjectService subjectService,
            UserContext userContext)
        {
            _subjectService = subjectService;
            _userContext = userContext;
        }


        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Subjects")]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> Subjects()
        {

            var subjects = await _subjectService.GetSubjects(_userContext.Id);

            return Ok(subjects);

        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Subject")]
        public async Task<ActionResult<SubjectDetailsDto>> Subject(int subjectId)
        {

            var subject = await _subjectService.GetSubjectDetails(subjectId);

            return Ok(subject);

        }



        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<SubjectDto>> CreateSubject(SubjectDto subjectDto)
        {

            var newSubject = await _subjectService.CreateSubject(subjectDto);

            return Ok(newSubject);

        }


        [Authorize(Roles = "Teacher")]
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<SubjectDto>> UpdateSubject(UpdateSubjectDto subjectDto)
        {

            var updatedSubject = await _subjectService.UpdateSubject(subjectDto);

            return Ok(updatedSubject);

        }


        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("Students")]
        public async Task<ActionResult> AddStudents(int subjectId, List<int> studentIds)
        {

            await _subjectService.AddStudents(subjectId, studentIds);

            return Ok();

        }

    }
}
