using SharedKernel.Shared;

namespace DtoLayer.GuestBookDtos;

public class GuestBookDto : IHasId
{
    public Guid Id { get; set; }
    public string AuthProvider { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorAvatarUrl { get; set; } = string.Empty;
    public string? AuthorProfileUrl { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
