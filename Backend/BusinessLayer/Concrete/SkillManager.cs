using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.SkillDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class SkillManager :GenericManager<Skill,SkillDto,CreateSkillDto,UpdateSkillDto> ,ISkillService
{
    private readonly ISkillDal _skillDal;

    public SkillManager(ISkillDal skillDal, IMapper mapper) : base(skillDal, mapper)
    {
        _skillDal = skillDal;
    }

    public async Task<SkillDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _skillDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;

        await _skillDal.UpdateAsync(entity, cancellationToken: cancellationToken);
        await _skillDal.SaveAsync(cancellationToken);

        return _mapper.Map<SkillDto>(entity);
    }
}
