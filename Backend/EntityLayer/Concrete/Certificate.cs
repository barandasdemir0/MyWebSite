using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class Certificate:BaseEntity
    {
        public string CertificateName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuingCompany { get; set; } = string.Empty;
        public string CertificateDescription { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }

    }
}
