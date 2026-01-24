using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.MessageDto
{
    public class MessageListDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
