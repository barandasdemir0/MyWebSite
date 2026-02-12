using DtoLayer.SiteSettingDtos;
using EntityLayer.Concrete;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

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
