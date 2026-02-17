using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.CertificateDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class CertificateManager : GenericManager<Certificate,CertificateDto,CreateCertificateDto,UpdateCertificateDto>,ICertificateService
{
    private readonly ICertificateDal _certificateDal;

    public CertificateManager(ICertificateDal certificateDal, IMapper mapper) : base(certificateDal, mapper)
    {
        _certificateDal = certificateDal;
    }

  

    public async Task<List<CertificateDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetAllAdminAsync(tracking: false,cancellationToken:cancellationToken);
        return _mapper.Map<List<CertificateDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public override async Task<List<CertificateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetAllAsync(tracking: false,cancellationToken:cancellationToken);
        return _mapper.Map<List<CertificateDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<CertificateDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.RestoreDeleteByIdAsync(guid,cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        await _certificateDal.UpdateAsync(entity, cancellationToken);
        await _certificateDal.SaveAsync(cancellationToken);
        return _mapper.Map<CertificateDto>(entity);
    }

}
