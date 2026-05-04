using SharedKernel.Shared;

namespace DtoLayer.EducationDtos;

public record EducationDto : IHasId
{
    public Guid Id { get; init; }
    public string EducationDegree { get; init; } = string.Empty;
    public DateTime? EducationStartDate { get; init; }
    public DateTime? EducationFinishDate { get; init; }
    public string EducationSchoolName { get; init; } = string.Empty;
    public string EducationDescription { get; init; } = string.Empty;
    public int? DisplayOrder { get; init; }
    public bool IsDeleted { get; init; } = false;
}
