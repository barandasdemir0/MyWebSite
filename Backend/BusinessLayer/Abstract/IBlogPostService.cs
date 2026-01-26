using CV.EntityLayer.Entities;
using DtoLayer.AboutDto;
using DtoLayer.BlogpostDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IBlogPostService : IGenericService< BlogPostDto, BlogPostListDto, CreateBlogPostDto,UpdateBlogPostDto>
    {
    }
}
