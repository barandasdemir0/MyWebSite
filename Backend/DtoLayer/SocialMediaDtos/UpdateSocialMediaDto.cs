using SharedKernel.Shared;

namespace DtoLayer.SocialMediaDtos;

public class UpdateSocialMediaDto : IHasId
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string SocialMediaName { get; set; } = string.Empty;
    public string SocialMediaUrl { get; set; } = string.Empty;
    public string SocialMediaIcon { get; set; } = string.Empty;
}
