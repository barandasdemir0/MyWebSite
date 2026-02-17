using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IJobSkillCategoryDal:IGenericRepository<JobSkillCategory>
{
    Task<JobSkillCategory?> RestoreDeleteByIdAsync(Guid guid,
        CancellationToken cancellationToken = default);
}
