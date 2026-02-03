using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.GuestBookDtos;

public class CreateGuestBookDto
{

    public string AuthProvider { get; set; } = string.Empty;
    public string AuthProviderId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorAvatarUrl { get; set; } = string.Empty;
    public string? AuthorProfileUrl { get; set; }
    public string Message { get; set; } = string.Empty;
}
