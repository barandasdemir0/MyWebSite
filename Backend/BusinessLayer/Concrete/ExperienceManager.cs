using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ExperienceDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class ExperienceManager : GenericManager<Experience,ExperienceDto,CreateExperienceDto,UpdateExperienceDto>,IExperienceService
{

    private readonly IExperienceDal _experienceDal;

    public ExperienceManager(IExperienceDal experienceDal, IMapper mapper) : base(experienceDal, mapper)
    {
        _experienceDal = experienceDal;
    }

    public async Task<List<ExperienceDto>> GetAllAdminAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _experienceDal.GetAllAdminAsync(tracking: false,cancellationToken:cancellationToken);
        return _mapper.Map<List<ExperienceDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<ExperienceDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _experienceDal.RestoreDeleteByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        await _experienceDal.UpdateAsync(entity, cancellationToken);
        await _experienceDal.SaveAsync(cancellationToken);
        return _mapper.Map<ExperienceDto>(entity);
    }

}
