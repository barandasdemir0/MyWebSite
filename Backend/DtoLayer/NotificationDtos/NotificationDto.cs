using SharedKernel.Shared;

namespace DtoLayer.NotificationDtos;

public class NotificationDto : IHasId
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;    // "comment", "project", "system", "user"
    public string Message { get; set; } = string.Empty;  // "Ahmet Y. yorum yaptı"
    public bool IsRead { get; set; } = false;
}
