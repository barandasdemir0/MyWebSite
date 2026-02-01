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
            config.NewConfig<BlogPost, BlogpostDto.BlogPostDto>().Map(x=>x.Topics,y=>y.BlogTopics.Select(z=>z.Topic.Name));
            config.NewConfig<BlogPost, BlogpostDto.BlogPostListDto>().Map(x=>x.Topics,y=>y.BlogTopics.Select(z=>z.Topic.Name));
            config.NewConfig<CreateBlogPostDto, BlogPost>().Ignore(x => x.Id);
            config.NewConfig<UpdateBlogPostDto, BlogPost>().Ignore(x => x.Id);
        }
    }
}
