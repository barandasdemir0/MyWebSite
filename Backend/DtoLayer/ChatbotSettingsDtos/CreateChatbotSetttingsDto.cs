using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.ChatbotSettingsDtos;

public class CreateChatbotSetttingsDto
{
    public string Type { get; set; } = string.Empty;    // "comment", "project", "system", "user"
    public string Message { get; set; } = string.Empty;  // "Ahmet Y. yorum yaptı"
    public bool IsRead { get; set; } = false;
}
