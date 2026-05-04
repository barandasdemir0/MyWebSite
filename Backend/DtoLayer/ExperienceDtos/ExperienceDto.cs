using SharedKernel.Shared;

namespace DtoLayer.ExperienceDtos;

public record ExperienceDto : IHasId
{
    public Guid Id { get; init; }
    public string ExperienceTitle { get; init; } = string.Empty;
    public DateTime? ExperienceStartDate { get; init; }
    public DateTime? ExperienceFinishDate { get; init; }
    public string ExperienceCompanyName { get; init; } = string.Empty;
    public string ExperienceDescription { get; init; } = string.Empty;
    public int? DisplayOrder { get; init; }   // Sıralama için
    public bool IsDeleted { get; init; } = false;
    public string? AiSummary { get; set; }

}
