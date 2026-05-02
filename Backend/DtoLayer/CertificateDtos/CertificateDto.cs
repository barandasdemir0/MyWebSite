using SharedKernel.Shared;

namespace DtoLayer.CertificateDtos;

public record CertificateDto : IHasId
{
    public Guid Id { get; init; }  
    public string CertificateName { get; init; } = string.Empty;
    public DateTime? IssueDate { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string IssuingCompany { get; init; } = string.Empty;
    public string CertificateDescription { get; init; } = string.Empty;
    public int? DisplayOrder { get; init; }
    public bool IsDeleted { get; init; } = false;
}
