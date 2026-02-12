using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.HeroDtos;
using DtoLayer.SiteSettingDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class SiteSettingsManager : ISiteSettingsService
{
    private readonly ISiteSettingsDal _siteSettingsDal;
    private readonly IMapper _mapper;

    public SiteSettingsManager(ISiteSettingsDal siteSettingsDal, IMapper mapper)
    {
        _siteSettingsDal = siteSettingsDal;
        _mapper = mapper;
    }

    public async Task<SiteSettingDto> AddAsync(CreateSiteSettingDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SiteSettings>(dto);
        await _siteSettingsDal.AddAsync(entity, cancellationToken);
        await _siteSettingsDal.SaveAsync(cancellationToken);
        return _mapper.Map<SiteSettingDto>(entity);
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _siteSettingsDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity != null)
        {
            await _siteSettingsDal.DeleteAsync(entity, cancellationToken);
            await _siteSettingsDal.SaveAsync(cancellationToken);
        }
    }

    public async Task<List<SiteSettingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _siteSettingsDal.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<SiteSettingDto>>(entity);
    }

    public async Task<SiteSettingDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {

        var entity = await _siteSettingsDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<SiteSettingDto>(entity);
    }

    public async Task<SiteSettingDto?> UpdateAsync(Guid guid, UpdateSiteSettingDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _siteSettingsDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _siteSettingsDal.UpdateAsync(entity, cancellationToken);
        await _siteSettingsDal.SaveAsync(cancellationToken);
        return _mapper.Map<SiteSettingDto>(entity);
    }
}
