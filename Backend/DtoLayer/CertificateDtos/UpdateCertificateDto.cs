using SharedKernel.Shared;

namespace DtoLayer.CertificateDtos;

public class UpdateCertificateDto : IHasId
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string CertificateName { get; set; } = string.Empty;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string IssuingCompany { get; set; } = string.Empty;
    public string CertificateDescription { get; set; } = string.Empty;
    public int? DisplayOrder { get; set; }
}
