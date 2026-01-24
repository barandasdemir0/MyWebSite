using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
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
    }
}
