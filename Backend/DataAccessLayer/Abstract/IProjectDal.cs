using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IProjectDal:IGenericRepository<Project>
    {
        Task<Project?> RestoreDeleteByIdAsync(Guid guid);
    }
}
