using DtoLayer.ContactDtos;
using DtoLayer.SocialMediaDtos;

namespace WebUILayer.Models;

public class FooterViewModel
{
    public List<SocialMediaDto> SocialMedias { get; set; } = new();
    public ContactDto? Contacts { get; set; } 
}
