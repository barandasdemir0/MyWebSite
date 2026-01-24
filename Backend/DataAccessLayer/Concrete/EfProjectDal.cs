using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfProjectDal : GenericRepository<Project>, IProjectDal
    {
        public EfProjectDal(AppDbContext context) : base(context)
        {
        }
    }
}
