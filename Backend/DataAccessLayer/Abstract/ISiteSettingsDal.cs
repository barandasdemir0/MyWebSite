using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface ISiteSettingsDal:IGenericRepository<SiteSettings>
{
    Task<SiteSettings?> GetSingleAsync(CancellationToken cancellationToken = default);
}
