using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface ISocialMediaDal:IGenericRepository<SocialMedia>
    {
        Task<SocialMedia?> RestoreDeleteByIdAsync(Guid guid,
        CancellationToken cancellationToken = default);
    }
}
