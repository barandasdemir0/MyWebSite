using DtoLayer.AboutDtos;
using DtoLayer.ContactDtos;
using DtoLayer.JobSkillCategoryDtos;
using DtoLayer.JobSkillsDtos;
using DtoLayer.SiteSettingDtos;

namespace WebUILayer.Models;

public class AboutViewModel
{
    public AboutDto? About { get; set; }

    public List<JobSkillDto> jobSkillDtos { get; set; } = new();
    public List<JobSkillCategoryDto> jobSkillCategoryDtos { get; set; } = new();
    public ContactDto? Contact { get; set; }
    public SiteSettingDto? SiteSetting { get; set; }

}
