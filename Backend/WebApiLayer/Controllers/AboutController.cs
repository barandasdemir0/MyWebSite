using BusinessLayer.Abstract;
using DtoLayer.AboutDto;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutController : ControllerBase
{
    private readonly IAboutService _aboutService;

    public AboutController(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateAbout([FromBody] CreateAboutDto createAboutDto)
    {

        var result = await _aboutService.AddAsync(createAboutDto);
        return Ok("İşlem Başarılı");
    }


}
