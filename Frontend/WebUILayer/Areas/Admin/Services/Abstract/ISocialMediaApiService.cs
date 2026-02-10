using DtoLayer.SocialMediaDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface ISocialMediaApiService : IGenericApiService<SocialMediaDto, CreateSocialMediaDto, UpdateSocialMediaDto>
    {
        Task<List<SocialMediaDto>> GetAdminAllAsync();
        Task RestoreAsync(Guid guid);
    }
}
