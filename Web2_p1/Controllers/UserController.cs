using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public UserController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
    {
        var user = new IdentityUser { UserName = dto.Username, Email = dto.Username };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            if (dto.Roles != null)
                await _userManager.AddToRolesAsync(user, dto.Roles);

            return Ok("Register Successful! Let login!");
        }
        return BadRequest("Something wrong!");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenRepository.CreateJWTToken(user, roles.ToList());
            return Ok(new LoginResponseDTO { JwtToken = token });
        }
        return BadRequest("Username or password incorrect");
    }
}
