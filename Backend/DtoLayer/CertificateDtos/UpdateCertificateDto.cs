using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.CertificateDtos;

public class UpdateCertificateDto
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string CertificateName { get; set; } = string.Empty;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string IssuingCompany { get; set; } = string.Empty;
    public string CertificateDescription { get; set; } = string.Empty;
    public int? DisplayOrder { get; set; }
}
