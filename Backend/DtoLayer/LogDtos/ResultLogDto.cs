using SharedKernel.Shared;

namespace DtoLayer.LogDtos;

public class ResultLogDto:IHasId
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? LogEvent { get; set; }
    public string? Exception { get; set; }
}
