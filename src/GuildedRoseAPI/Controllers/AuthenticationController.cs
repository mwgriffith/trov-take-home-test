using GuildedRoseAPI.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GuildedRoseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// Returns a valid Jwt token if a valid user is passed in.
        /// </summary>
        /// <param name="user">A user that will be tested.</param>
        /// <returns>A Jwt token if the user is correct, a badrequest otherwise.</returns>
        [HttpPost]
        public ActionResult GetToken([FromForm] UserModel user)
        {
            if (IsValidUserandPassword(user.username, user.password))
            {
                return Ok(GenerateToken(user.username));
            } 
            return BadRequest();
        }
                
        
        /// <summary>
        /// Checks to see if a username and password combo are valid.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to chec.</param>
        /// <returns>True if they are valid, False otherwise.</returns>
        /// <remarks>Since this is just a test the validation is extremely minimal.  Would never put this in production.</remarks>
        private Boolean IsValidUserandPassword(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }


        /// <summary>
        /// Generates a Jwt token with the given username.
        /// </summary>
        /// <param name="username">The username of the user given the token.</param>
        /// <returns>A Jwt Token.</returns>
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