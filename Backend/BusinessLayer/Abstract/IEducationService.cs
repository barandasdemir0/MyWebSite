using CV.EntityLayer.Entities;
using DtoLayer.EducationDtos;

namespace BusinessLayer.Abstract;

public interface IEducationService:IGenericService<Education,EducationDto,CreateEducationDto,UpdateEducationDto>
{
    Task<EducationDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<EducationDto>> GetAllAdminAsync( CancellationToken cancellationToken = default);
}
