using DtoLayer.BlogPostDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicBlogPostApiService:IPublicReadApiService<BlogPostDto>
{
    Task<List<BlogPostDto>> GetLatestAsync(int count);
}
