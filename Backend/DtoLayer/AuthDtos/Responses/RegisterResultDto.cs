namespace DtoLayer.AuthDtos.Responses;

public record RegisterResultDto
{
    public bool Success { get; init; }
    public List<string>? Errors { get; init; }
}
