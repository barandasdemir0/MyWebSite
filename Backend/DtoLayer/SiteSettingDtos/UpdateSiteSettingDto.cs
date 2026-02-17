namespace DtoLayer.SiteSettingDtos;

public class UpdateSiteSettingDto
{
    public Guid Id { get; set; }
    public string? CvFileUrlTr { get; set; }
    public string? CvFileUrlEn { get; set; }
    public string WorkStatus { get; set; } = string.Empty;  // "İşe Açık Yazısı"
    public bool IsAvailable { get; set; } // "İşe Açık checkbox"

    // (SEO):
    public string? SiteTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public string? SiteKeywords { get; set; }
}
