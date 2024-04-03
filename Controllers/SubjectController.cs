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
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<SubjectDto>> CreateSubject(SubjectDto subjectDto)
        {

            var newSubject = await _subjectService.CreateSubject(subjectDto);

            return Ok(newSubject);

        }

    }
}
