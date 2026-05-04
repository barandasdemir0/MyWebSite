using DtoLayer.GuestBookDtos;

namespace WebUILayer.Areas.Admin.Models;

public class GuestBookIndexViewModel:BasePaginationViewModel
{
    public List<GuestBookDto> Messages { get; set; } = new();
    public string ActiveTab { get; set; } = "pending";
}
