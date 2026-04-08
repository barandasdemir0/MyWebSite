using SharedKernel.Shared;

namespace DtoLayer.ContactDtos;

public record ContactDto : IHasId
{
    public Guid Id { get; init; }
    public string? CvFileUrl { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string LocationPicture { get; init; } = string.Empty;
    public string ContactTitle { get; init; } = string.Empty;
    public string ContactText { get; init; } = string.Empty;
    public string SuccessMessageText { get; init; } = string.Empty;
    public string WorkStatus { get; init; } = string.Empty;  
    public bool IsAvailable { get; init; }
}
