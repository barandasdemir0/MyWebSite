using CV.EntityLayer.Entities;
using DtoLayer.AboutDtos;
using DtoLayer.BlogpostDtos;
using DtoLayer.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IBlogPostService : IGenericService<BlogPost, BlogPostDto, CreateBlogPostDto, UpdateBlogPostDto>
    {
        //bu metot blogpostDto(full içerik details içindir)
        Task<BlogPostDto?> GetDetailsByIdAsync(Guid guid); // --> ama bu yöntemi sadece admin paneli için kullanacağız  sebebide idye göre getireceği için ama biz sluga göre çekmek istiyoruz

        //bu yöntem ile slug ile çektik daha sağlıklı bir yöntem oldu
        Task<BlogPostDto?> GetBySlugAsync(string slug);

        Task<BlogPostListDto?> RestoreAsync(Guid guid);

        Task<PagedResult<BlogPostListDto>> GetAllAdminAsync(PaginationQuery query);



    }
}
