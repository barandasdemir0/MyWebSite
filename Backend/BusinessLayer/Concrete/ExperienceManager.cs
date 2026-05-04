using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ExperienceDtos;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class ExperienceManager : GenericManager<Experience,ExperienceDto,CreateExperienceDto,UpdateExperienceDto>,IExperienceService
{

    private readonly IExperienceDal _experienceDal;

    public ExperienceManager(IExperienceDal experienceDal, IMapper mapper, IUnitOfWork unitOfWork) : base(experienceDal, mapper, unitOfWork)
    {
        _experienceDal = experienceDal;
    }

    public async Task<List<ExperienceDto>> GetAllAdminAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _experienceDal.GetAllAsync(tracking: false, options: new QueryOptions<Experience>
        {
            OrderBy = x => x.DisplayOrder,
            IgnoreQueryFilters = true // Bu satır artık bir sihirbaz gibi çalışacak!
        }, cancellationToken:cancellationToken);
        return _mapper.Map<List<ExperienceDto>>(entity);
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ExperienceDto>(entity);
    }

}
