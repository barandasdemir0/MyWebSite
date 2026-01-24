using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.ContactDto
{
    public class CreateContactDto
    {
        public string? CvFileUrl { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string LocationPicture { get; set; } = string.Empty;
        public string ContactTitle { get; set; } = string.Empty;
        public string ContactText { get; set; } = string.Empty;
        public string SuccessMessageText { get; set; } = string.Empty;
        public string WorkStatus { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
