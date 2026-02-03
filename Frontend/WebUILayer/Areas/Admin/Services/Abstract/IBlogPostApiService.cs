using DtoLayer.BlogpostDtos;
using DtoLayer.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IBlogPostApiService:IGenericApiService<BlogPostDto,CreateBlogPostDto, BlogPostDto>
    {
        Task<BlogPostListDto?> GetDetailById(Guid guid);
        Task<BlogPostListDto?> GetDetailBySlug(string slug);
        Task RestoreAsync(Guid guid);
        Task<PagedResult<BlogPostDto>> GetAllAdminAsync(PaginationQuery query);
    }
}
