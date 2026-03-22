using CV.EntityLayer.Entities;
using DtoLayer.BlogPostDtos;
using SharedKernel.Shared;

//using DtoLayer.Shared;

namespace BusinessLayer.Abstract;

public interface IBlogPostService : IGenericService<BlogPost, BlogPostDto, CreateBlogPostDto, UpdateBlogPostDto>
{

    //bu yöntem ile slug ile çektik daha sağlıklı bir yöntem oldu
    Task<BlogPostDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<BlogPostListDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<PagedResult<BlogPostListDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<PagedResult<BlogPostListDto>> GetAllUserAsync(PaginationQuery query, CancellationToken cancellationToken = default);

    //count ile kaçtane blog yazısı istiyorsun bunu hallettik
    Task<List<BlogPostDto>> GetLatestAsync(int count, CancellationToken cancellationToken=default);



}
