using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Concrete;

public class AuthManager : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthManager> _logger;


    public AuthManager(UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<IdentityRole<Guid>> roleManager, ILogger<AuthManager> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _logger = logger;
    }



    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user==null)
        {
            return Fail("Geçersiz Eposta veya şifre");
        }



        if (await _userManager.IsLockedOutAsync(user))
        {
            return Fail("Hesabınız Geçiçi Olarak Kilitlendi");
        }


        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            await _userManager.AccessFailedAsync(user);
            return Fail("Geçersiz e-posta veya şifre");
        }

        await _userManager.ResetAccessFailedCountAsync(user);


        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {

            return new LoginResultDto
            {
                Success = true,
                RequiresTwoFactor = true,
                UserId = user.Id.ToString()

            };
        }

        return new LoginResultDto
        {
            Success = true,
            Token = await CreateJwtAsync(user)

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
            EmailConfirmed = false
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

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = "User"
            });
        }
        await _userManager.AddToRoleAsync(user, "User");

       
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
            expires: DateTime.UtcNow.AddHours(1),
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
