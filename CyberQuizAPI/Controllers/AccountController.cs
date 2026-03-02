using CyberQuiz.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CyberQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IWebHostEnvironment environment,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _environment = environment;
        _logger = logger;
    }

    public sealed record LoginDto([Required] string UserName, [Required] string Password, bool RememberMe = false);
    public sealed record UserDto(string Id, string? UserName);
    public sealed record AuthErrorDto(string Code);

    // POST: api/account/login
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
    {
        // Model validation is handled by [ApiController] and data annotations on LoginDto
        var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, dto.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                _logger.LogWarning("Login blocked due to lockout for user {UserName}", dto.UserName);
            }
            else if (result.RequiresTwoFactor)
            {
                _logger.LogWarning("Login requires two-factor authentication for user {UserName}", dto.UserName);
            }
            else if (result.IsNotAllowed)
            {
                _logger.LogWarning("Login not allowed for user {UserName}", dto.UserName);
            }
            else
            {
                _logger.LogWarning("Failed login attempt for user {UserName}", dto.UserName);
            }

            // Avoid leaking account state (lockout/2FA/not-allowed) via response codes.
            return Unauthorized(new AuthErrorDto("auth_failed"));
        }

        // Retrieve user to return minimal profile information for UI
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user is null)
        {
            // Sign-in succeeded but user not found (shouldn't happen) - return generic success without body
            _logger.LogWarning("User signed in successfully but could not be loaded by username {UserName}", dto.UserName);
            return Ok();
        }

        _logger.LogInformation("User {UserId} ({UserName}) logged in", user.Id, user.UserName);

        return Ok(new UserDto(user.Id, user.UserName));
    }

    // POST: api/account/logout
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var userId = _userManager.GetUserId(User);
        await _signInManager.SignOutAsync();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _logger.LogInformation("User {UserId} logged out", userId);
        }
        else
        {
            _logger.LogInformation("Logout called without a resolved user id");
        }

        return NoContent();
    }

    // GET: api/account/me
    // Simple endpoint to verify that cookie authentication + [Authorize] works.
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Me()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
            return Unauthorized();

        return Ok(new UserDto(user.Id, user.UserName));
    }

    // GET: api/account/dev/seed-status
    // Development-only helper endpoint to verify that the seeded users exist.
    [HttpGet("dev/seed-status")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DevSeedStatus()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        _logger.LogInformation("Dev seed status check requested");

        var hasUser = await _userManager.FindByNameAsync("user") is not null;
        var hasAdmin = await _userManager.FindByNameAsync("admin") is not null;

        return Ok(new { hasUser, hasAdmin });
    }
}
