using Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.AuthenticationModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace KovanTaskApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        public AccountController(TokenService tokenService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginRequestDto loginDto)
        {
            var isEmailEntered = (new EmailAddressAttribute().IsValid(loginDto.Username));
            User user = null;
            if (isEmailEntered)
                user = await _userManager.FindByEmailAsync(loginDto.Username);
            else
                user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }
            var token = await _tokenService.GenerateToken(user);

            return new UserResponseDto
            {
                Email = user.Email,
                Token = token,
                CookieExpiryDate = DateTime.Now.AddDays(30)
            };
        }
    }
}
