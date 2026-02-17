using DtoLayer.CertificateDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.CertificateValidator;

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
            .NotNull()
            .WithMessage("Tarih Boş Olamaz")
          .LessThanOrEqualTo(DateTime.UtcNow)
          .WithMessage("Sertifika tarihi bugünden ileri olamaz");

        RuleFor(x=>x.DisplayOrder).NotNull()
            .WithMessage("Boş geçilemez")
            .GreaterThan(0)
              .WithMessage("Sıralama 0'dan büyük olmalıdır");
    }
}
