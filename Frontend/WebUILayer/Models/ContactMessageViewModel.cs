using DtoLayer.ContactDtos;
using DtoLayer.MessageDtos;
using DtoLayer.SocialMediaDtos;

namespace WebUILayer.Models;

public class ContactMessageViewModel
{
    public UpdateContactDto? contactDto { get; set; } 
    public CreateMessageDto? createMessageDto { get; set; }
    public List<SocialMediaDto> SocialMediaDtos { get; set; } = new();
}
