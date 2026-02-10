using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract;

public interface IJobSkillDal:IGenericRepository<JobSkill>
{
    Task<JobSkill?> RestoreDeleteByIdAsync(Guid guid);
}
