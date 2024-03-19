using GeoQuest.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GeoQuest.Services;
using GeoQuest.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace GeoQuest.Controllers.Authentication
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;



        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        // POST api/<AuthenticationController>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {

            var account = await this._authService.Login(loginDto);
            var response = new Response();

            var userRole = (account.Role == (int)UserRole.Teacher) ? "Teacher" : "Student";

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(account.Id)),
                    new Claim(ClaimTypes.Email,account.Email),
                    new Claim(ClaimTypes.Role,  userRole),
                    // Any additional custom claim --> new Claim("DotNetMania", "Code")

                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(45) // Set the expiration time here (e.g., 45 minutes from now)
            });

            response.Message = "Prijava uspješna";
            return Ok(response);
        }


        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var response = new Response
            {
                Message = "Odjava uspješna"
            };

            return Ok(response);
        }



        [Authorize]
        [HttpGet]
        [Route("restricted")]
        public string Restricted()
        {
            return "Ovdje moraš bit ulogiran";
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("teacher")]
        public string Teacher()
        {
            return "Ovdje moraš biti teacher";
        }

    }
}
