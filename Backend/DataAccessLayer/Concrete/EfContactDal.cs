using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfContactDal : GenericRepository<Contact>, IContactDal
    {
        public EfContactDal(AppDbContext context) : base(context)
        {
        }
    }
}
