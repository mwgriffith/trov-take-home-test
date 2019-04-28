using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace GuildedRoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public ActionResult GetToken(string username, string password)
        {
            if (IsValidUserandPassword(username, password))
            {
                return Ok(GenerateToken(username));
            } 
            return BadRequest();
        }


        private Boolean IsValidUserandPassword(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }


        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                // add username into the claims
                new Claim(ClaimTypes.Name, username),
                // add the current time of the token to the claim.
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                // add the expiration time of the token to the claim.
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var credentials = new SigningCredentials( 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"])), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                new JwtHeader(credentials),
                new JwtPayload(claims)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}