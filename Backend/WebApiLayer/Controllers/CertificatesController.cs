using BusinessLayer.Abstract;
using DtoLayer.CertificateDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
public sealed class CertificatesController:CrudController<CertificateDto,CreateCertificateDto,UpdateCertificateDto>
{
    private readonly ICertificateService _certificateService;

    public CertificatesController(ICertificateService certificateService) : base(certificateService)
    {
        _certificateService = certificateService;
    }

   

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin( CancellationToken cancellationToken)
    {
        var values = await _certificateService.GetAllAdminAsync( cancellationToken);
        return Ok(values);
    }


    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _certificateService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }


}
