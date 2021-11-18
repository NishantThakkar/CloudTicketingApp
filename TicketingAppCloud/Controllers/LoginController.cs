using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TicketingAppCloud.DBModels;
using TicketingAppCloud.Service;

namespace TicketingAppCloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly TicketingDbContext _ticketingDbContext;
        private readonly IConfiguration _config;

        public LoginController(TicketingDbContext ticketingDbContext, IConfiguration config)
        {
            _ticketingDbContext = ticketingDbContext;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] User login)
        {
            if (_ticketingDbContext.Users.Any(x => x.UserName == login.UserName && x.Password == login.Password))
            {
                var token = new JwtService(_config).GenerateSecurityToken(login.UserName);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized();
            }            
        }
    }
}
