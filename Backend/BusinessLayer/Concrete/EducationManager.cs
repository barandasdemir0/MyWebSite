using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.EducationDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class EducationManager : GenericManager<Education,EducationDto,CreateEducationDto,UpdateEducationDto>,IEducationService
{
    private readonly IEducationDal _educationDal;

    public EducationManager(IEducationDal educationDal, IMapper mapper) : base(educationDal, mapper)
    {
        _educationDal = educationDal;
    }

 

    public async Task<List<EducationDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.GetAllAdminAsync(tracking: false, cancellationToken:cancellationToken);
        return _mapper.Map<List<EducationDto>>(entity.OrderBy(x=>x.DisplayOrder));
    }



    public async Task<EducationDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.RestoreDeleteByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;

        await _educationDal.UpdateAsync(entity, cancellationToken);
        await _educationDal.SaveAsync(cancellationToken);

        return _mapper.Map<EducationDto>(entity);
    }

  
}
