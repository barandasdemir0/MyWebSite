namespace CV.EntityLayer.Entities;

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


    // SADECE Bakım Moduna özel alanlar
    public bool IsMaintenanceMode { get; set; } = false;
    public DateTime? MaintenanceEndTime { get; set; } // Bakım ne zaman bitecek?
    public string? MaintenanceMessage { get; set; } // "Veritabanı güncelleniyor" vb.
}
