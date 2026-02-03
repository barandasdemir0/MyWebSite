using DtoLayer.BlogpostDto;
using DtoLayer.TopicDto;

namespace WebUILayer.Areas.Admin.Models
{
    public class BlogPostIndexViewModel:BasePaginationViewModel
    {
        public List<BlogPostDto> blogPostDtos { get; set; } = new List<BlogPostDto>();
        public List<TopicDto> topicDtos { get; set; } = new List<TopicDto>();
    }
}
