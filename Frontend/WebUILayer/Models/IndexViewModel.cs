using DtoLayer.AboutDtos;
using DtoLayer.GithubRepoDtos;
using DtoLayer.GuestBookDtos;
using DtoLayer.HeroDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.SiteSettingDtos;
using DtoLayer.SkillDtos;
using DtoLayer.SocialMediaDtos;

namespace WebUILayer.Models;

public class IndexViewModel
{
    public SiteSettingDto? siteSettingDto { get; set; }
    public HeroDto? heroDto { get; set; }
    public AboutDto? aboutDto { get; set; }
    public List<SkillDto> skillDtos { get; set; } = new();
    public List<ProjectDto> projectListDtos { get; set; } = new();
    public List<GuestBookListDto> guestBookListDtos { get; set; } = new();
    public List<GithubRepoDto> githubRepoDtos { get; set; } = new();
    public List<SocialMediaDto> socialMediaDtos { get; set; } = new();
}
