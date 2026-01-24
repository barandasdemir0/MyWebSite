using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfSkillDal : GenericRepository<Skill>, ISkillDal
    {
        public EfSkillDal(AppDbContext context) : base(context)
        {
        }
    }
}
