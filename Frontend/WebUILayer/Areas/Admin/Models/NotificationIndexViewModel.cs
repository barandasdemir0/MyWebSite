using DtoLayer.NotificationDtos;

namespace WebUILayer.Areas.Admin.Models;

public class NotificationIndexViewModel:BasePaginationViewModel
{
    public List<NotificationDto>? NotificationDtos { get; set; }
}
