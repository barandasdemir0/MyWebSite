using BusinessLayer.Abstract;
using DtoLayer.GuestBookDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OAuthController : ControllerBase
{
    private readonly IGithubAuthService _githubAuthService;
    private readonly ILinkedinAuthService _linkedinAuthService;

    public OAuthController(IGithubAuthService githubAuthService, ILinkedinAuthService linkedinAuthService)
    {
        _githubAuthService = githubAuthService;
        _linkedinAuthService = linkedinAuthService;
    }

    [AllowAnonymous]
    [HttpPost("github")]
    public async Task<IActionResult> GithubLogin([FromBody]GithubAuthRequestDto githubAuthRequestDto)
    {
        var profile = await _githubAuthService.AuthenticateAsync(githubAuthRequestDto);
        if (profile==null)
        {
            return BadRequest("Geçersiz Github Yetki Kodu");
        }
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("linkedin")]
    public async Task<IActionResult> LinkedinLogin([FromBody]LinkedinAuthRequestDto linkedinAuthRequestDto)
    {
        var profile = await _linkedinAuthService.AuthenticateAsync(linkedinAuthRequestDto);
        if (profile==null)
        {
            return BadRequest("Linkedin Geersiz Yetki Kodu");
        }
        return Ok(profile);
    }
}
