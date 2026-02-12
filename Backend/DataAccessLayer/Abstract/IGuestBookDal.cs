using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IGuestBookDal:IGenericRepository<GuestBook>
    {
        Task<GuestBook?> RestoreDeleteByIdAsync(Guid guid,
        CancellationToken cancellationToken = default);
    }
}
