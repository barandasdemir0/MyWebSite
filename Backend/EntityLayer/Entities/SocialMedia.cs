namespace CV.EntityLayer.Entities;

public sealed class SocialMedia:BaseEntity
{
    public string SocialMediaName { get; set; } = string.Empty;
    public string SocialMediaUrl { get; set; } = string.Empty;
    public string SocialMediaIcon { get; set; } = string.Empty;
}
