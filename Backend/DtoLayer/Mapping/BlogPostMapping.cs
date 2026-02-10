using CV.EntityLayer.Entities;
using DtoLayer.BlogpostDtos;
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
            // BlogPostDto için TopicIds ekle
            config.NewConfig<BlogPost, BlogpostDtos.BlogPostDto>()
                .Map(x => x.Topics, y => y.BlogTopics.Select(z => z.Topic.Name).ToList())
                .Map(x => x.TopicIds, y => y.BlogTopics.Select(z => z.TopicId).ToList());
            // BlogPostListDto için TopicIds YOK (zaten property yok)
            config.NewConfig<BlogPost, BlogpostDtos.BlogPostListDto>()
                .Map(x => x.Topics, y => y.BlogTopics.Select(z => z.Topic.Name).ToList());
            config.NewConfig<CreateBlogPostDto, BlogPost>().Ignore(x => x.Id);
            config.NewConfig<UpdateBlogPostDto, BlogPost>().Ignore(x => x.Id);
        }
    }
}
