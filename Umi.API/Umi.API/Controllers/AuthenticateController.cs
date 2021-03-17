using System;
using System.Collections.Generic;
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
using Umi.API.Models;

namespace Umi.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticateController(
            IConfiguration configuration, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async  Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // 1. verify user/pwd
            var loginResult = await _signInManager.PasswordSignInAsync(
                loginDto.email,
                loginDto.password,
                false,
                false
                );

            if (!loginResult.Succeeded)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(loginDto.email);

            // 2. create JWT
            // HEADER 
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            
            // PAYLOAD 
            // claim: permission item of user, atomic, claims -> authorisation -> identity
            var claims = new List<Claim>
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                // new Claim(ClaimTypes.Role, "Admin"), 
            };
            var roleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }
            

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
            var user = new ApplicationUser()
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