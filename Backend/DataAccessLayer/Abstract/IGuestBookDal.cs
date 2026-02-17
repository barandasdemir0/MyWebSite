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


        Task<(List<GuestBook> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default);

        Task<(List<GuestBook> Items, int TotalCount)> GetUserListPagesAsync(int page, int size, CancellationToken cancellationToken = default);
    }
}
