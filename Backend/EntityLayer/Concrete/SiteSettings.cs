using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.Concrete;

public sealed class SiteSettings :BaseEntity
{
    public string? CvFileUrlTr { get; set; }
    public string? CvFileUrlEn { get; set; }
    public string WorkStatus { get; set; } = string.Empty;  // "İşe Açık Yazısı"
    public bool IsAvailable { get; set; } // "İşe Açık checkbox"

    // Yeni (SEO):
    public string? SiteTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public string? SiteKeywords { get; set; }
}
