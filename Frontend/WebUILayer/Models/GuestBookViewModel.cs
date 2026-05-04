using DtoLayer.GuestBookDtos;
using SharedKernel.Shared;

namespace WebUILayer.Models;

public class GuestBookViewModel
{
    public PagedResult<GuestBookDto> Messages { get; set; } = new();
    public CreateGuestBookDto? GuestUser { get; set; }
}
