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

    public async Task<CertificateDto> AddAsync(CreateCertificateDto dto)
    {
        var entity = _mapper.Map<Certificate>(dto);
        await _certificateDal.AddAsync(entity);
        await _certificateDal.SaveAsync();
        return _mapper.Map<CertificateDto>(entity);
    }

    public async Task DeleteAsync(Guid guid)
    {
        var entity = await _certificateDal.GetByIdAsync(guid);
        if (entity != null)
        {
            await _certificateDal.DeleteAsync(entity);
            await _certificateDal.SaveAsync();
        }
    }

    public async Task<List<CertificateDto>> GetAllAdminAsync()
    {
        var entity = await _certificateDal.GetAllAdminAsync(tracking: false);
        return _mapper.Map<List<CertificateDto>>(entity);
    }

    public async Task<List<CertificateDto>> GetAllAsync()
    {
        var entity = await _certificateDal.GetAllAsync(tracking:false);
        return _mapper.Map<List<CertificateDto>>(entity);
    }
    

    public async Task<CertificateDto?> GetByIdAsync(Guid guid)
    {
        var entity = await _certificateDal.GetByIdAsync(guid, tracking: false);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<CertificateDto>(entity);
    }

    public async Task<CertificateDto?> RestoreAsync(Guid guid)
    {
        var entity = await _certificateDal.RestoreDeleteByIdAsync(guid);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        await _certificateDal.UpdateAsync(entity);
        await _certificateDal.SaveAsync();
        return _mapper.Map<CertificateDto>(entity);
    }

    public async Task<CertificateDto?> UpdateAsync(Guid guid, UpdateCertificateDto dto)
    {
        var entity = await _certificateDal.GetByIdAsync(guid);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _certificateDal.UpdateAsync(entity);
        await _certificateDal.SaveAsync();
        return _mapper.Map<CertificateDto>(entity);
    }
}
