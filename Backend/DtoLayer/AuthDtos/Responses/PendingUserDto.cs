namespace DtoLayer.AuthDtos.Responses;

public record PendingUserDto
{
    public string UserId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
