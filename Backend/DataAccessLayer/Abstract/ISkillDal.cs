using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface ISkillDal:IGenericRepository<Skill>
{
    Task<Skill?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default);
}
