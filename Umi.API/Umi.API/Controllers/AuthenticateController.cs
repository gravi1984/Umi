using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Umi.API.Dtos;

namespace Umi.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticateController(
            IConfiguration configuration, 
            UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // 1. verify user/pwd

            // 2. create JWT
            // HEADER 
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            

            // PAYLOAD 
            // claim: permission item of user, atomic, claims -> authorisation -> identity
            var claims = new[]
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, "fake_user_id"),
                new Claim(ClaimTypes.Role, "Admin"), 
            };

            // SIG
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authenticate:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);
            
            var token = new JwtSecurityToken(
                issuer:_configuration["Authenticate:Issuer"],
                audience:_configuration["Authenticate:Audience"],
                claims,
                notBefore:DateTime.Now,
                expires:DateTime.Now.AddDays(1),
                signingCredentials
                
            );
            
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);


            // 3. return 200 ok + JWT
            return Ok(tokenStr);

        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) // DB, async
        {
            // 1. create User
            var user = new IdentityUser()
            {
                UserName =  registerDto.Email,
                Email = registerDto.Email
            };
            
            // 2. hash pwd, save User
            // hash pwd + save user to db
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            // 3. return 200
            return Ok();

        }
        
    }
}