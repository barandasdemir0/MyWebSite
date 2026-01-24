using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class About : BaseEntity
    {
        public string ProfileImage { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Greeting { get; set; } = string.Empty; // kısa merhaba ben ile ilgili başlayan kısımlar için
        public string Bio { get; set; } = string.Empty; // uzun mesajımın olduğu yer
        public int ProjectCount { get; set; } 
        public int ExperienceYear { get; set; } 
        public int ProjectDrink { get; set; } 

    }
}
