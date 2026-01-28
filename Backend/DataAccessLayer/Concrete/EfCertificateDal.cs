using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfCertificateDal : GenericRepository<Certificate>, ICertificateDal
    {
        public EfCertificateDal(AppDbContext context) : base(context)
        {
        }

        public Task<Certificate?> RestoreDeleteByIdAsync(Guid guid)
        {
            return _context.Certificates.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}
