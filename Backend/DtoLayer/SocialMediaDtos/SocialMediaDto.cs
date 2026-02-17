using SharedKernel.Shared;

namespace DtoLayer.SocialMediaDtos;

public class SocialMediaDto : IHasId
{
    public Guid Id { get; set; }
    public string SocialMediaName { get; set; } = string.Empty;
    public string SocialMediaUrl { get; set; } = string.Empty;
    public string SocialMediaIcon { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
