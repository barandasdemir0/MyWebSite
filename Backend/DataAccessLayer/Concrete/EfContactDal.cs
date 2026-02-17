using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfContactDal : GenericRepository<Contact>, IContactDal
{
    public EfContactDal(AppDbContext context) : base(context)
    {
    }

    public async Task<Contact?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }
}
