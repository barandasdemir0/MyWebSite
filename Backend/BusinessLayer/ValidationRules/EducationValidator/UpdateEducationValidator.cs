using DtoLayer.EducationDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.EducationValidator
{
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
        }
    }
}
