using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.SocialMediaDtos;

public class UpdateSocialMediaDto
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string SocialMediaName { get; set; } = string.Empty;
    public string SocialMediaUrl { get; set; } = string.Empty;
    public string SocialMediaIcon { get; set; } = string.Empty;
}
