
using BusinessLayer.Abstract;
using DtoLayer.CertificateDtos;
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
    public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
    {
        var values = await _certificateService.GetAllAsync( cancellationToken);
        return Ok(values);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin( CancellationToken cancellationToken)
    {
        var values = await _certificateService.GetAllAdminAsync( cancellationToken);
        return Ok(values);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var values = await _certificateService.GetByIdAsync(id, cancellationToken);
        if (values==null)
        {
            return NotFound();
        }
        return Ok(values);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCertificateDto createCertificateDto, CancellationToken cancellationToken)
    {
        var result = await _certificateService.AddAsync(createCertificateDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateCertificateDto updateCertificateDto, CancellationToken cancellationToken)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var values = await _certificateService.UpdateAsync(id,updateCertificateDto, cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var values = await _certificateService.GetByIdAsync(id, cancellationToken);
        if (values== null)
        {
            return NotFound();
        }
        await _certificateService.DeleteAsync(id, cancellationToken);
        return Ok(values);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _certificateService.RestoreAsync(id, cancellationToken);
        return Ok(entity);
    }






}
