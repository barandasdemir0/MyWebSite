using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfGithubRepoDal : GenericRepository<GithubRepo>, IGithubRepoDal
    {
        public EfGithubRepoDal(AppDbContext context) : base(context)
        {
        }
    }
}
