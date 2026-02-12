using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IBlogPostDal:IGenericRepository<BlogPost>
    {
        Task<BlogPost?> RestoreDeletedByIdAsync(Guid guid,
        CancellationToken cancellationToken = default);

        Task<(List<BlogPost> Items,int TotalCount)> GetAdminListPagesAsync(int page, int size,Guid? topicId,
        CancellationToken cancellationToken = default);
    }
}
