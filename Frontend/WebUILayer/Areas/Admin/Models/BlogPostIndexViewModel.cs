using DtoLayer.BlogpostDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models
{
    public class BlogPostIndexViewModel:BasePaginationViewModel
    {
        public List<BlogPostDto> blogPostDtos { get; set; } = new List<BlogPostDto>();
        public List<TopicDto> topicDtos { get; set; } = new List<TopicDto>();
    }
}
