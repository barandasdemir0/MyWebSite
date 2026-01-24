using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class Contact:BaseEntity
    {
        // CV
        public string? CvFileUrl { get; set; }

        //// Sosyal Medya
        //public string? GithubUrl { get; set; }
        //public string? LinkedInUrl { get; set; }
        //public string? TwitterUrl { get; set; }
        //public string? InstagramUrl { get; set; }

        // İletişim
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string LocationPicture { get; set; } = string.Empty;
        public string ContactTitle { get; set; } = string.Empty;
        public string ContactText { get; set; } = string.Empty;
        public string SuccessMessageText { get; set; } = string.Empty;
        public string WorkStatus { get; set; } = string.Empty;  // "İşe Açık"
        public bool IsAvailable { get; set; } 
    }
}
