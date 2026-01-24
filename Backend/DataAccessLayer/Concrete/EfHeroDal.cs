using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfHeroDal : GenericRepository<Hero>, IHeroDal
    {
        public EfHeroDal(AppDbContext context) : base(context)
        {
        }
    }
}
