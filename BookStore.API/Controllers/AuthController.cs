using System.Threading.Tasks;
using BookStore.API.Data;
using BookStore.API.Dtos;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            _repo = repo;
            _configuration= config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterationDto userDto)
        {
             userDto.UserName = userDto.UserName.ToLower();
             if(await _repo.UserExists(userDto.UserName))
                return BadRequest("User exist before");

              var userToCreate= new User(){
                UserName = userDto.UserName
              };   

              var createdUser = await _repo.Register(userToCreate,userDto.Password);
              return StatusCode(201); 
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto usrDto)
        {
          var userFromRepo = await _repo.Login(usrDto.UserName,usrDto.Password);
          if(userFromRepo == null)
            return Unauthorized();
            var claims = new []{
              new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
              new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
            var tokenDiscriptor = new SecurityTokenDescriptor{
              Subject = new ClaimsIdentity(claims),
              Expires = DateTime.Now.AddDays(1),
              SigningCredentials = creds
            };

            IdentityModelEventSource.ShowPII = true;
            var tokenHandler = new JwtSecurityTokenHandler();
            var token= tokenHandler.CreateToken(tokenDiscriptor);
            return Ok(new {token = tokenHandler.WriteToken(token)});
        }
    }
}