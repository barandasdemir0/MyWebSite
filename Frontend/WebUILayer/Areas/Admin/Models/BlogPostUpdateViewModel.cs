using DtoLayer.BlogpostDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models
{
    public class BlogPostUpdateViewModel
    {
        public BlogPostDto BlogPost { get; set; } = new();  // TEK! Liste değil!
        public List<TopicDto> TopicList { get; set; } = new();
    }
}
