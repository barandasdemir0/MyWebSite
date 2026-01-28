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

        public async Task<GuestBook?> RestoreDeleteByIdAsync(Guid guid)
        {
            return await _context.GuestBooks.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}
