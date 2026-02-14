using CV.EntityLayer.Entities;
using DtoLayer.AboutDtos;
using DtoLayer.BlogpostDtos;
using SharedKernel.Shared;

//using DtoLayer.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface IBlogPostService : IGenericService<BlogPost, BlogPostDto, CreateBlogPostDto, UpdateBlogPostDto>
{
    //bu metot blogpostDto(full içerik details içindir)
    Task<BlogPostDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default); // --> ama bu yöntemi sadece admin paneli için kullanacağız  sebebide idye göre getireceği için ama biz sluga göre çekmek istiyoruz

    //bu yöntem ile slug ile çektik daha sağlıklı bir yöntem oldu
    Task<BlogPostDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<BlogPostListDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<PagedResult<BlogPostListDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<PagedResult<BlogPostListDto>> GetAllUserAsync(PaginationQuery query, CancellationToken cancellationToken = default);

    //count ile kaçtane blog yazısı istiyorsun bunu hallettik
    Task<List<BlogPostDto>> GetLatestAsync(int count, CancellationToken cancellationToken=default);



}
