using DtoLayer.CertificateDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.CertificateValidator
{
    public class CreateCertificateValidator:AbstractValidator<CreateCertificateDto>
    {
        public CreateCertificateValidator()
        {
            RuleFor(x => x.CertificateName).NotEmpty()
               .WithMessage("Bu Alan Boş Geçilemez")
               .MaximumLength(100)
               .WithMessage("Bu Alan En fazla 100 Karakter olmalıdır.");

            RuleFor(x => x.IssuingCompany).NotEmpty()
               .WithMessage("Bu Alan Boş Geçilemez")
               .MaximumLength(100)
               .WithMessage("Bu Alan En fazla 100 Karakter olmalıdır.");

            RuleFor(x => x.CertificateDescription).NotEmpty()
               .WithMessage("Bu Alan Boş Geçilemez")
               .MinimumLength(10)
               .WithMessage("En az 10 karakter veri girmeniz gerekiyor");

            RuleFor(x => x.IssueDate)
              .LessThanOrEqualTo(DateTime.UtcNow)
              .WithMessage("Sertifika tarihi bugünden ileri olamaz");
        }
    }
}
