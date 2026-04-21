using DtoLayer.ContactDtos;
using DtoLayer.MessageDtos;

namespace WebUILayer.Models;

public class ContactMessageViewModel
{
    public UpdateContactDto? contactDto { get; set; } 
    public CreateMessageDto? createMessageDto { get; set; }
}
