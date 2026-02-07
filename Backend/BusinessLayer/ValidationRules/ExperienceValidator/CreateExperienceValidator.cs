using DtoLayer.ExperienceDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.ExperienceValidator
{
    public class CreateExperienceValidator:AbstractValidator<CreateExperienceDto>
    {
        public CreateExperienceValidator()
        {
            RuleFor(x => x.ExperienceTitle)
                .NotEmpty()
                .WithMessage("Bu Alan Boş geçilemez")
                .MaximumLength(100)
                .WithMessage("Bu Alan 100 Karakterden fazla olamaz");

            RuleFor(x => x.ExperienceCompanyName)
                .NotEmpty()
                .WithMessage("Bu Alan Boş geçilemez")
                .MaximumLength(200)
                .WithMessage("Bu Alan 200 Karakterden fazla olamaz");

            RuleFor(x => x.ExperienceTitle)
                .NotEmpty()
                .WithMessage("Bu Alan Boş geçilemez")
                .MinimumLength(10)
                .WithMessage("Bu Alan 10 Karakterden az olamaz");


            RuleFor(x => x.ExperienceStartDate)
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
}
