using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IEducationDal:IGenericRepository<Education>
    {
        Task<Education?> RestoreDeleteByIdAsync(Guid guid);
    }
}
