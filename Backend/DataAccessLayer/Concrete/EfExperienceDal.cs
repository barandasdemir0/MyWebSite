using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfExperienceDal : GenericRepository<Experience>, IExperienceDal
    {
        public EfExperienceDal(AppDbContext context) : base(context)
        {
        }
    }
}
