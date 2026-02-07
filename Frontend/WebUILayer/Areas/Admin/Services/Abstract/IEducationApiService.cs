using DtoLayer.EducationDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IEducationApiService:IGenericApiService<EducationDto,CreateEducationDto,UpdateEducationDto>
    {
        Task<List<EducationDto>> GetAllAdminAsync();
        Task RestoreAsync(Guid guid);
    }
}
