using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract;

public interface IJobSkillCategoryDal:IGenericRepository<JobSkillCategory>
{
    Task<JobSkillCategory?> RestoreDeleteByIdAsync(Guid guid,
        CancellationToken cancellationToken = default);
}
