using Azure;
using GeoQuest.DTOs;
using GeoQuest.DTOs.Extensions;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Models;
using GeoQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoQuest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService,
            UserContext userContext)
        {
            _accountService = accountService;
            _userContext = userContext;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {

            var accountId = await this._accountService.createAccount(registerDto);

            var response = new Response
            {
                Message = "Račun je uspješno registriran"
            };

            return Ok(response);

        }



        [Authorize]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult<AccountDto>> GetAccountDetails()
        {
            var details = await _accountService.GetAccount(_userContext.Id);
            return Ok(details);
        }



        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("Students")]
        public async Task<ActionResult<StudentDto>> GetStudentAccounts(int subjectId)
        {
            var accounts = await _accountService.GetAccounts(Enums.UserRole.Student, subjectId);

            List<StudentDto> studentDtoList = accounts.Select(a => a.AsStudentDto()).ToList();

            return Ok(studentDtoList);
        }



    }
}
