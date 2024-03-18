using Azure;
using GeoQuest.DTOs;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Services;
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



    }
}
