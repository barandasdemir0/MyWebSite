using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class Contact:BaseEntity
    {
      

        // İletişim
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string LocationPicture { get; set; } = string.Empty;
        public string ContactTitle { get; set; } = string.Empty;
        public string ContactText { get; set; } = string.Empty;
        public string SuccessMessageText { get; set; } = string.Empty;
 
    }
}
