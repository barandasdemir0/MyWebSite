using CV.EntityLayer.Entities;
using DtoLayer.SiteSettingDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface ISiteSettingsService:IGenericService<SiteSettings,SiteSettingDto,CreateSiteSettingDto,UpdateSiteSettingDto>
{
    Task<SiteSettingDto?> GetSingleAsync(CancellationToken cancellationToken = default);

    Task<SiteSettingDto> SaveAsync(UpdateSiteSettingDto updateSiteSettingDto, CancellationToken cancellation = default);
}
