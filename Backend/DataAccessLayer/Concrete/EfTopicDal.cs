using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfTopicDal : GenericRepository<Topic>, ITopicDal
    {
        public EfTopicDal(AppDbContext context) : base(context)
        {
        }
    }
}
