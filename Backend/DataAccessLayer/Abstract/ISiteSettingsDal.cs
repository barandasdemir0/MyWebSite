using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract;

public interface ISiteSettingsDal:IGenericRepository<SiteSettings>
{
    Task<SiteSettings?> GetSingleAsync(CancellationToken cancellationToken = default);
}
