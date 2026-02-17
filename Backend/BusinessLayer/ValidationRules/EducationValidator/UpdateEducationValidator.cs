using DtoLayer.EducationDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.EducationValidator;

public class UpdateEducationValidator:AbstractValidator<UpdateEducationDto>
{
    public UpdateEducationValidator()
    {
        RuleFor(x => x.EducationDegree)
           .NotEmpty()
           .WithMessage("Bu Alan Boş Geçilemez")
           .MaximumLength(50)
           .WithMessage("Bu Alan 50 Karakterden fazla olamaz");

        RuleFor(x => x.EducationSchoolName)
            .NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MaximumLength(100)
            .WithMessage("Bu Alan 100 Karakterden fazla olamaz");

        RuleFor(x => x.EducationDescription)
            .NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MinimumLength(10)
            .WithMessage("Açıklama En az 10 Karakter olmalıdır");

        RuleFor(x => x.EducationStartDate)
             .NotNull()
            .WithMessage("Tarih Boş Olamaz")
       .LessThanOrEqualTo(DateTime.UtcNow)
       .WithMessage("Sertifika tarihi bugünden ileri olamaz");

        RuleFor(x => x.DisplayOrder).NotNull()
        .WithMessage("Boş geçilemez")
        .GreaterThan(0)
          .WithMessage("Sıralama 0'dan büyük olmalıdır");
    }
}
