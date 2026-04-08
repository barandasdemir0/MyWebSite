using SharedKernel.Shared;

namespace DtoLayer.ExperienceDtos;

public class UpdateExperienceDto:IHasId
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir çünkü kullanıcı neyi güncelleyeceğini bilmeli bu sebeple hidden üzerinden gönderilir
    public string ExperienceTitle { get; set; } = string.Empty;
    public DateTime? ExperienceStartDate { get; set; }
    public DateTime? ExperienceFinishDate { get; set; }
    public string ExperienceCompanyName { get; set; } = string.Empty;
    public string ExperienceDescription { get; set; } = string.Empty;
    public int? DisplayOrder { get; set; }   // Sıralama için
}
