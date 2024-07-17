using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using api.DTOs.request;
using api.Filters;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
         this.userManager = userManager;   
         this.tokenService = tokenService;
         this.signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (UserLoginDTO userLoginDTO) {
            var user = await userManager.Users.FirstOrDefaultAsync(user => user.UserName == userLoginDTO.Username);
            if (user == null) {
                return Unauthorized("Invalid username!");
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password,false);
            if (!result.Succeeded) return Unauthorized("Username not found or password incorrect");
            return Ok(
                new NewUserDTO {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = tokenService.CreateToken(user)

                }
            );
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO) {
            var appUser = new AppUser {
                UserName = userRegisterDTO.Username,
                Email = userRegisterDTO.Email
            };
            var createdUser = await userManager.CreateAsync(appUser,userRegisterDTO.Password);
            if (createdUser.Succeeded) {
                var roleResult = await userManager.AddToRoleAsync(appUser,"User");
                if (roleResult.Succeeded) {
                    return Ok(
                        new NewUserDTO {
                            Username = appUser.UserName,
                            Email = appUser.Email,
                            Token = tokenService.CreateToken(appUser)
                        }
                    );
                } else {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            return StatusCode(500, createdUser.Errors);
        }
        
    }
}