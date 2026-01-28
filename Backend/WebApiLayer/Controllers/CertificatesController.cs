
using BusinessLayer.Abstract;
using DtoLayer.CertificateDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class CertificatesController:ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificatesController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _certificateService.GetAllAsync();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var values = await _certificateService.GetByIdAsync(id);
        if (values==null)
        {
            return NotFound();
        }
        return Ok(values);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCertificateDto createCertificateDto)
    {
        var result = await _certificateService.AddAsync(createCertificateDto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateCertificateDto updateCertificateDto)
    {
        updateCertificateDto.Id = id;
        var values = await _certificateService.UpdateAsync(updateCertificateDto);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var values = await _certificateService.GetByIdAsync(id);
        if (values== null)
        {
            return NotFound();
        }
        await _certificateService.DeleteAsync(id);
        return Ok(values);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var entity = await _certificateService.RestoreAsync(id);
        return Ok(entity);
    }






}
