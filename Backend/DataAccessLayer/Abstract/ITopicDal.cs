using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface ITopicDal : IGenericRepository<Topic>
    {
        Task<Topic?> RestoreDeleteByIdAsync(Guid guid,
        CancellationToken cancellationToken = default);
    }
}
