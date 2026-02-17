using DtoLayer.BlogPostDtos;
using DtoLayer.GuestBookDtos;
using DtoLayer.MessageDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models;

public class DashboardIndexViewModel
{
    public List<ProjectDto> projectListDtos { get; set; } = new List<ProjectDto>();
    public List<BlogPostDto> blogPostListDtos { get; set; } = new List<BlogPostDto>();

    //public int totalBlogPosts { get; set; }
    //public List<MessageListDto> messageListDtos { get; set; } = new List<MessageListDto>();
    //public List<GuestBookListDto> guestBookListDtos { get; set; } = new List<GuestBookListDto>();
}
