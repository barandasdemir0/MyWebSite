using BusinessLayer.Abstract;
using DtoLayer.AuthDtos.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogOut(CancellationToken cancellation)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!; //türkçe : Kullanıcı kimliğini al
        await _tokenService.RevokeTokensAsync(userId, cancellation); //türkçe : Kullanıcının tüm tokenlarını iptal et
        return Ok(); //türkçe : Başarılı bir şekilde çıkış yapıldı

    }


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto, CancellationToken cancellationToken)
    {
        var deviceInfo = Request.Headers["User-Agent"].ToString(); //türkçe : Cihaz bilgisi olarak User-Agent başlığını al user-agent başlığı, istemcinin tarayıcı ve işletim sistemi gibi bilgilerini içerir
        var result = await _tokenService.RefreshTokenAsync(dto, deviceInfo, cancellationToken); //türkçe : Token yenileme işlemi için token servisinden sonuç al
        if (result.Success)
        {
            return Ok(result);
        }
        return Unauthorized(result.Error);
    }

}
