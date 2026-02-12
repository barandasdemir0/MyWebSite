using DtoLayer.SiteSettingDtos;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface ISiteSettingsService:IGenericService<SiteSettings,SiteSettingDto,CreateSiteSettingDto,UpdateSiteSettingDto>
{
}
