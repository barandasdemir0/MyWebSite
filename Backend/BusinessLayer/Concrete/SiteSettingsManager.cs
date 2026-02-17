using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.SiteSettingDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class SiteSettingsManager : GenericManager<SiteSettings,SiteSettingDto,CreateSiteSettingDto,UpdateSiteSettingDto> ,ISiteSettingsService
{
    private readonly ISiteSettingsDal _siteSettingsDal;

    public SiteSettingsManager(ISiteSettingsDal siteSettingsDal, IMapper mapper) : base(siteSettingsDal, mapper)
    {
        _siteSettingsDal = siteSettingsDal;
    }



    public async Task<SiteSettingDto?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _siteSettingsDal.GetSingleAsync(cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<SiteSettingDto>(entity);
    }

    public async Task<SiteSettingDto> SaveAsync(UpdateSiteSettingDto updateSiteSettingDto, CancellationToken cancellation = default)
    {
        var entity = await _siteSettingsDal.GetSingleAsync(cancellation);
        if (entity == null)
        {
            entity = _mapper.Map<SiteSettings>(updateSiteSettingDto);
            await _siteSettingsDal.AddAsync(entity);
        }
        else
        {
            _mapper.Map(updateSiteSettingDto, entity);
            await _siteSettingsDal.UpdateAsync(entity, cancellation);
        }

        await _siteSettingsDal.SaveAsync(cancellation);
        return _mapper.Map<SiteSettingDto>(entity);


    }
}
