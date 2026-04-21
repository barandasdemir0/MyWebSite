using DtoLayer.MessageDtos;
using SharedKernel.Enums;

namespace WebUILayer.Areas.Admin.Models;

public class MessageIndexViewModel:BasePaginationViewModel
{
    public List<MessageDto> messageDtos { get; set; } = new();
    public Dictionary<string, int> FolderCounts { get; set; } = new();
    public string ActiveCategory { get; set; } = "inbox";
}
