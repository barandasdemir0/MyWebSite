using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.Concrete
{
    public sealed class SocialMedia:BaseEntity
    {
        public string SocialMediaName { get; set; } = string.Empty;
        public string SocialMediaUrl { get; set; } = string.Empty;
        public string SocialMediaIcon { get; set; } = string.Empty;
    }
}
