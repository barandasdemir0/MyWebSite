using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfBlogPostDal : GenericRepository<BlogPost>, IBlogPostDal
    {
        public EfBlogPostDal(AppDbContext context) : base(context)
        {
        }
    }
}
