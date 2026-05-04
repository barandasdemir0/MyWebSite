using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.SkillDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class SkillManager :GenericManager<Skill,SkillDto,CreateSkillDto,UpdateSkillDto> ,ISkillService
{
    private readonly ISkillDal _skillDal;

    public SkillManager(ISkillDal skillDal, IMapper mapper, IUnitOfWork unitOfWork) : base(skillDal, mapper, unitOfWork)
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SkillDto>(entity);
    }
}
