using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.MessageDto
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty; // Düzeltildi: MessageDetail -> Body

        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
        public string Folder { get; set; } = string.Empty; // Enum string olarak gelecek

        public DateTime CreatedAt { get; set; }  // CreatedAt BaseEntity'den geliyor
    }
}
