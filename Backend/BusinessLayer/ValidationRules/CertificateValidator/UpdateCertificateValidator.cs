using DtoLayer.CertificateDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.CertificateValidator
{
    public class UpdateCertificateValidator : AbstractValidator<UpdateCertificateDto>
    {
        public UpdateCertificateValidator()
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
               .WithMessage("Bu Alan Boş Geçilemez");
        }
    }
}
