using DtoLayer.BlogPostDtos;
using SharedKernel.Shared;

namespace WebUILayer.Services.Abstract;

public interface IPublicBlogPostApiService:IPublicReadApiService<BlogPostDto>
{
    Task<List<BlogPostDto>> GetLatestAsync(int count,string? topic = null);
    Task<BlogPostDto?> GetBySlugAsync(string slug);
    Task<PagedResult<BlogPostDto>> GetAllPagedAsync(PaginationQuery paginationQuery);
}
