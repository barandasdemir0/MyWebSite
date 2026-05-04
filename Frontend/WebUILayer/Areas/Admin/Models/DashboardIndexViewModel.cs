using DtoLayer.BlogPostDtos;
using DtoLayer.GuestBookDtos;
using DtoLayer.MessageDtos;
using DtoLayer.ProjectDtos;

namespace WebUILayer.Areas.Admin.Models;

public class DashboardIndexViewModel
{
    public List<ProjectDto> projectListDtos { get; set; } = new List<ProjectDto>();
    public List<BlogPostDto> blogPostListDtos { get; set; } = new List<BlogPostDto>();

    //public int totalBlogPosts { get; set; }
    public List<MessageDto> messageListDtos { get; set; } = new();
    public List<GuestBookDto> guestBookListDtos { get; set; } = new ();
}
