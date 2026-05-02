using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.EducationDtos;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class EducationManager : GenericManager<Education,EducationDto,CreateEducationDto,UpdateEducationDto>,IEducationService
{
    private readonly IEducationDal _educationDal;

    public EducationManager(IEducationDal educationDal, IMapper mapper, IUnitOfWork unitOfWork) : base(educationDal, mapper, unitOfWork)
    {
        _educationDal = educationDal;
    }

 

    public async Task<List<EducationDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.GetAllAsync(tracking: false, options: new QueryOptions<Education>
        {
            OrderBy = x => x.DisplayOrder,
            IgnoreQueryFilters = true // Bu satır artık bir sihirbaz gibi çalışacak!
        }, cancellationToken:cancellationToken);
        return _mapper.Map<List<EducationDto>>(entity);
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EducationDto>(entity);
    }

  
}
