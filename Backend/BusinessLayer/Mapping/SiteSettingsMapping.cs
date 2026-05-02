using CV.EntityLayer.Entities;
using DtoLayer.SiteSettingDtos;
using Mapster;

namespace DtoLayer.Mapping;

public class SiteSettingsMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SiteSettings, SiteSettingDtos.SiteSettingDto>();
        config.NewConfig<CreateSiteSettingDto, SiteSettings>().Ignore(x=>x.Id);
        config.NewConfig<UpdateSiteSettingDto, SiteSettings>().Ignore(x=>x.Id);
    }
}
