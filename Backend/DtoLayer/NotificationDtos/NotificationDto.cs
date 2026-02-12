using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.NotificationDtos;

public class NotificationDto
{
    public string Type { get; set; } = string.Empty;    // "comment", "project", "system", "user"
    public string Message { get; set; } = string.Empty;  // "Ahmet Y. yorum yaptı"
    public bool IsRead { get; set; } = false;
}
