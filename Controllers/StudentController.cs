using GeoQuest.DTOs;
using GeoQuest.Middlewares.UserContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoQuest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly UserContext _userContext;
        //private readonly ISubjectService _subjectService;

        public StudentController(
            //ISubjectService subjectService,
            UserContext userContext)
        {
           // _subjectService = subjectService;
            _userContext = userContext;
        }

        /*[Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Subjects")]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> Subjects()
        {

            var subjects = await _subjectService.GetSubjects(_userContext.Id);

            return Ok(subjects);

        } */




    }
}
