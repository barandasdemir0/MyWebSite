using BusinessLayer.Abstract;
using DtoLayer.AboutDtos;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AboutsController : ControllerBase
{
    private readonly IAboutService _aboutService;

    public AboutsController(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }

    [HttpGet] //listeleme
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var values = await _aboutService.GetAllAsync(cancellationToken);
        return Ok(values);
    }

    [HttpGet("{id}")] //idye göre getirme
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var values = await _aboutService.GetByIdAsync(id, cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }

    [HttpPost] //ekleme
    public async Task<IActionResult> Create([FromBody] CreateAboutDto createAboutDto, CancellationToken cancellationToken)
    {
        var result = await _aboutService.AddAsync(createAboutDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); //bunu çağırarak ne kaydedildiğimi görmek istiyorum dedik yani başarılar kaydedildi yerine bize direkt o kaydı gösteriyor
    }

    [HttpPut("{id}")] //güncelleme
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateAboutDto updateAboutDto, CancellationToken cancellationToken)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var result = await _aboutService.UpdateAsync(id,updateAboutDto, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);

    }

    [HttpDelete("{id}")] //silme
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _aboutService.GetByIdAsync(id, cancellationToken);
        if (existing==null)
        {
            return NotFound();
        }
        await _aboutService.DeleteAsync(id, cancellationToken);
        return Ok(existing);

    }









}
