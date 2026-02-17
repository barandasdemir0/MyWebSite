using CV.EntityLayer.Entities;
using DtoLayer.SocialMediaDtos;

namespace BusinessLayer.Abstract;

public interface ISocialMediaService:IGenericService<SocialMedia,SocialMediaDto,CreateSocialMediaDto,UpdateSocialMediaDto>
{
    Task<SocialMediaDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<SocialMediaDto>> GetAllAdminAsync( CancellationToken cancellationToken = default);
}
