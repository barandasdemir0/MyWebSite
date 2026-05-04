using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.CertificateDtos;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class CertificateManager : GenericManager<Certificate, CertificateDto, CreateCertificateDto, UpdateCertificateDto>, ICertificateService
{
    private readonly ICertificateDal _certificateDal;

    public CertificateManager(ICertificateDal certificateDal, IMapper mapper,IUnitOfWork unitOfWork) : base(certificateDal, mapper,unitOfWork)
    {
        _certificateDal = certificateDal;
    }



    public async Task<List<CertificateDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetAllAsync(tracking: false, options: new QueryOptions<Certificate>
        {
            OrderBy = x => x.DisplayOrder,
            IgnoreQueryFilters = true // Sihir Admin'de olmalı!
        }, cancellationToken: cancellationToken);
        return _mapper.Map<List<CertificateDto>>(entity);
    }

    public override async Task<List<CertificateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetAllAsync(tracking: false, options: new QueryOptions<Certificate>
        {
            OrderBy = x => x.DisplayOrder
        }, cancellationToken: cancellationToken);
        return _mapper.Map<List<CertificateDto>>(entity);
    }

    public async Task<CertificateDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.RestoreDeleteByIdAsync(guid, cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        await _certificateDal.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CertificateDto>(entity);
    }

}