using CV.EntityLayer.Entities;
using DtoLayer.SkillDtos;

namespace BusinessLayer.Abstract;

public interface ISkillService:IGenericService<Skill,SkillDto,CreateSkillDto,UpdateSkillDto>
{
    Task<SkillDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);

}
