using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
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
    }
}
