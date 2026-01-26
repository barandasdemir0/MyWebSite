using DtoLayer.ExperienceDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.EducationValidator.ExperienceValidator
{
    public class UpdateExperienceValidator:AbstractValidator<UpdateExperienceDto>
    {
        public UpdateExperienceValidator()
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
        }
    }
}
