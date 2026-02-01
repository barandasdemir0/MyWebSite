using DtoLayer.BlogpostDto;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IBlogPostApiService:IGenericApiService<BlogPostDto,CreateBlogPostDto,UpdateBlogPostDto>
    {
        Task<BlogPostListDto?> GetDetailById(Guid guid);
        Task<BlogPostListDto?> GetDetailBySlug(string slug);
        Task RestoreAsync(Guid guid);
        Task<List<BlogPostDto>> GetAllAdminAsync();
    }
}
