using CV.EntityLayer.Entities;
using DtoLayer.AboutDto;
using DtoLayer.BlogpostDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IBlogPostService : IGenericService<BlogPostDto, BlogPostListDto, CreateBlogPostDto, UpdateBlogPostDto>
    {
        //bu metot blogpostDto(full içerik details içindir)
        Task<BlogPostDto?> GetDetailsByIdAsync(Guid guid); // --> ama bu yöntemi sadece admin paneli için kullanacağız  sebebide idye göre getireceği için ama biz sluga göre çekmek istiyoruz

        //bu yöntem ile slug ile çektik daha sağlıklı bir yöntem oldu
        Task<BlogPostDto?> GetBySlugAsync(string slug);


    }
}
