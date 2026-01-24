using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfEducationDal : GenericRepository<Education>, IEducationDal
    {
        public EfEducationDal(AppDbContext context) : base(context)
        {
        }
    }
}
