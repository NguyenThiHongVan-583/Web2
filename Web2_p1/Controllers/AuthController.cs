using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;

namespace Web2_p1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        #region Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (model.Roles != null && model.Roles.Any())
                await _userManager.AddToRolesAsync(user, model.Roles);

            return Ok("Register successful! You can now login.");
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user == null)
                return BadRequest("Invalid username or password");

            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
                return BadRequest("Invalid username or password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenRepository.CreateJWTToken(user, roles.ToList());

            return Ok(new LoginResponseDTO { JwtToken = token });
        }
        #endregion
    }
}
