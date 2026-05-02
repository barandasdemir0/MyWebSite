using DtoLayer.ContactDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicContactApiService : PublicReadApiService<ContactDto>, IPublicContactApiService
{
    public PublicContactApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "contacts")
    {
    }
}
