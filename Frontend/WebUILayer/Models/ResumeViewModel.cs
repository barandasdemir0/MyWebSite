using DtoLayer.CertificateDtos;
using DtoLayer.EducationDtos;
using DtoLayer.ExperienceDtos;
using DtoLayer.SiteSettingDtos;

namespace WebUILayer.Models;

public class ResumeViewModel
{
    public List<CertificateDto> certificateDtos { get; set; } = new();
    public List<EducationDto> educationDtos { get; set; } = new();
    public List<ExperienceDto> experienceDtos { get; set; } = new();
    public SiteSettingDto? siteSettingDto { get; set; }
}
