using CV.EntityLayer.Entities;
using DtoLayer.ExperienceDtos;

namespace BusinessLayer.Abstract;

public interface IExperienceService:IGenericService<Experience,ExperienceDto,CreateExperienceDto,UpdateExperienceDto>
{
    Task<ExperienceDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<ExperienceDto>> GetAllAdminAsync( CancellationToken cancellationToken = default);
}
