using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.CertificateDtos;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class CertificateManager : ICertificateService
{
    private readonly ICertificateDal _certificateDal;
    private readonly IMapper _mapper;

    public CertificateManager(ICertificateDal certificateDal, IMapper mapper)
    {
        _certificateDal = certificateDal;
        _mapper = mapper;
    }

    public async Task<CertificateDto> AddAsync(CreateCertificateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Certificate>(dto);
        await _certificateDal.AddAsync(entity,cancellationToken);
        await _certificateDal.SaveAsync( cancellationToken);
        return _mapper.Map<CertificateDto>(entity);
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity != null)
        {
            await _certificateDal.DeleteAsync(entity, cancellationToken);
            await _certificateDal.SaveAsync(cancellationToken);
        }
    }

    public async Task<List<CertificateDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetAllAdminAsync(tracking: false,cancellationToken:cancellationToken);
        return _mapper.Map<List<CertificateDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<List<CertificateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetAllAsync(tracking: false,cancellationToken:cancellationToken);
        return _mapper.Map<List<CertificateDto>>(entity.OrderBy(x => x.DisplayOrder));
    }


    public async Task<CertificateDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetByIdAsync(guid, tracking: false, cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<CertificateDto>(entity);
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

    public async Task<CertificateDto?> UpdateAsync(Guid guid, UpdateCertificateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _certificateDal.GetByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _certificateDal.UpdateAsync(entity,cancellationToken);
        await _certificateDal.SaveAsync(cancellationToken);
        return _mapper.Map<CertificateDto>(entity);
    }
}
