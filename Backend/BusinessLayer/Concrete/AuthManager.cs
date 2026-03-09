using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Concrete;

public class AuthManager : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthManager(UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
        _roleManager = roleManager;
    }

  

    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user== null|| !await _userManager.CheckPasswordAsync(user,loginDto.Password))
        {
            return Fail("Geçersiz Eposta veya şifre");
        }
        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            return new LoginResultDto
            {
                Success = true,
                Token = await CreateJwtAsync(user)
            };
        }

        return new LoginResultDto
        {
            Success = true,
            RequiresTwoFactor = true,
            UserId = user.Id.ToString()
        };
    }

    public async Task RevokeTokensAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user!=null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
        }
    } 

    public async Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return new RegisterResultDto
            {
                Success = false,
                Errors = result.Errors.Select(x => x.Description).ToList()
            };

        }
        return new RegisterResultDto
        {
            Success = true
        };
    }


    private async Task<string> CreateJwtAsync(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email??""),
            new Claim(ClaimTypes.Name,$"{user.Name}{user.Surname}")

        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));


        var token = new JwtSecurityToken
            (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static LoginResultDto Fail(string error)
    {
        return new LoginResultDto
        {
            Success = false,
            Error = error

        };
    }

}
