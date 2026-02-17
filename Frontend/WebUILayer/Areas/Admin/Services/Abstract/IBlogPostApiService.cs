using DtoLayer.BlogPostDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IBlogPostApiService:IGenericApiService<BlogPostDto,CreateBlogPostDto, UpdateBlogPostDto>
    {
        Task<BlogPostListDto?> GetDetailById(Guid guid);
        Task<BlogPostListDto?> GetDetailBySlug(string slug);
        Task RestoreAsync(Guid guid);
        Task<PagedResult<BlogPostDto>> GetAllAdminAsync(PaginationQuery query);
        Task<List<BlogPostDto>> GetLatestAsync(int count);
    }
}
