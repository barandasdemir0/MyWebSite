using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IJobSkillDal:IGenericRepository<JobSkill>
{
    Task<JobSkill?> RestoreDeleteByIdAsync(Guid guid,
        CancellationToken cancellationToken = default);
}
