using DtoLayer.EducationDtos;
using DtoLayer.ExperienceDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IExperienceApiService:IGenericApiService<ExperienceDto,CreateExperienceDto,UpdateExperienceDto>
    {
        Task<List<ExperienceDto>> GetAllAdminAsync();
        Task RestoreAsync(Guid guid);
    }
}
