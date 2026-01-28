using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IExperienceDal:IGenericRepository<Experience>
    {
        Task<Experience?> RestoreDeleteByIdAsync(Guid guid);
    }
}
