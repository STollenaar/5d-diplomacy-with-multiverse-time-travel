using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Controllers;

[ApiController]
[Route("auth")]
public class UserController(
    ILogger<UserController> logger,
    IUserRepository userRepository,
    IConfiguration configuration
) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var user = _userRepository.GetUserByUsernameAndPassword(loginRequest.Username, loginRequest.Password);
        if (user == null)
        {
            _logger.LogInformation("Invalid login attempt");
            return Unauthorized("Invalid username or password");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] User user)
    {
        _userRepository.CreateUser(user);
        return Ok();
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("Jwt:Key", "JWT Key is not configured.");
        }

        var keyBytes = Encoding.ASCII.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}