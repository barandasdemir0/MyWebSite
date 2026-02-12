using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete;

public class EfSiteSettingsDal : GenericRepository<SiteSettings>, ISiteSettingsDal
{
    public EfSiteSettingsDal(AppDbContext context) : base(context)
    {
    }
}
