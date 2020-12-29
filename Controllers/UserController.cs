using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Prism.Services;
using Prism.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Prism.Data;

namespace Prism.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseCtx database;
        private UserService userService;
        private string secret = "Cheia secreta pentru token. Nu e asa secreta.";

        public UserController(DatabaseCtx context)
        {
            this.database = context;
            this.userService = new UserService(context);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] LoginUser model)
        {
            var user = userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.UserId,
                Name = user.Name,
                Role = user.Role,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User model)
        {
            userService.Register(model, model.Password);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            try
            {
                int id = GetUserId();
                var user = userService.GetById(id);
                return Ok(new {
                    id = user.UserId,
                    email = user.Email,
                    name = user.Name,
                    role = user.Role,
                });
            }
            catch (ApplicationException)
            {
                return NotFound("User not  found");
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UserUpdate newUser)
        {
            try
            {
                int id = GetUserId();
                var user = userService.Update(id,newUser);
                return Ok(new
                {
                    id = user.UserId,
                    email = user.Email,
                    name = user.Name,
                    role = user.Role,
                });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("change-password")]
        public IActionResult UpdatePassword([FromBody] ChangePasswordModel passwords)
        {
            try
            {
                int id = GetUserId();
                userService.UpdatePassword(id, passwords.OldPassword, passwords.NewPassword);
                return Ok();
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { message = e.Message });
            }
            
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            try
            {
                int id = GetUserId();
                userService.Delete(id);
                return Ok();
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        private int GetUserId()
        {
            try
            {
                return Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            }
            catch (Exception e)
            {
                throw new ApplicationException("UserId was not found");
            }
        }
    }
}
