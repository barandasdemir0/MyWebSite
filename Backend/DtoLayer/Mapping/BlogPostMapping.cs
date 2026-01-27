using CV.EntityLayer.Entities;
using DtoLayer.BlogpostDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class BlogPostMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BlogPost, BlogpostDto.BlogPostDto>();
            config.NewConfig<BlogPost, BlogpostDto.BlogPostListDto>();
            config.NewConfig<CreateBlogPostDto, BlogPost>().Ignore(x => x.Id);
            config.NewConfig<UpdateBlogPostDto, BlogPost>().Ignore(x => x.Id);
        }
    }
}
