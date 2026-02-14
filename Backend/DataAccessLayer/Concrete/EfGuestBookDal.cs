using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfGuestBookDal : GenericRepository<GuestBook>, IGuestBookDal
    {
        public EfGuestBookDal(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<GuestBook> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            IQueryable<GuestBook> query = _context.GuestBooks.AsNoTrackingWithIdentityResolution().IgnoreQueryFilters();
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<GuestBook> Items, int TotalCount)> GetUserListPagesAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            IQueryable<GuestBook> query = _context.GuestBooks.AsNoTrackingWithIdentityResolution();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (items, totalCount);

        }

        public async Task<GuestBook?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default)
        {
            return await _context.GuestBooks.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
        }
    }
}
