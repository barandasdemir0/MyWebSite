using CV.EntityLayer.Entities;

namespace EntityLayer.Entities;

public class SystemLog : BaseEntity
{
    public string Message { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? LogEvent { get; set; }
}