using BusinessLayer.Abstract;
using DtoLayer.AboutDto;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutsController : ControllerBase
{
    private readonly IAboutService _aboutService;

    public AboutsController(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }

    [HttpGet] //listeleme
    public async Task<IActionResult> GetAll()
    {
        var values = await _aboutService.GetAllAsync();
        return Ok(values);
    }

    [HttpGet("{id}")] //idye göre getirme
    public async Task<IActionResult> GetById(Guid id)
    {
        var values = await _aboutService.GetByIdAsync(id);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }

    [HttpPost] //ekleme
    public async Task<IActionResult> Create([FromBody] CreateAboutDto createAboutDto)
    {
        var result = await _aboutService.AddAsync(createAboutDto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); //bunu çağırarak ne kaydedildiğimi görmek istiyorum dedik yani başarılar kaydedildi yerine bize direkt o kaydı gösteriyor
    }

    [HttpPut("{id}")] //güncelleme
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateAboutDto updateAboutDto)
    {
        updateAboutDto.Id = id;
        var result = await _aboutService.UpdateAsync(updateAboutDto);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);

    }

    [HttpDelete("{id}")] //silme
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _aboutService.GetByIdAsync(id);
        if (existing==null)
        {
            return NotFound();
        }
        await _aboutService.DeleteAsync(id);
        return Ok(existing);

    }









}
