using DtoLayer.SkillDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.SkillValidator
{
    public class CreateSkillValidator:AbstractValidator<CreateSkillDto>
    {
        public CreateSkillValidator()
        {
            RuleFor(x => x.SkillName)
                .NotEmpty()
                .WithMessage("Yetenekleriniz boş geçilemez")
                .MaximumLength(100)
                .WithMessage("100 Karakterden daha fazla yetenek ismi olamaz");

            RuleFor(x => x.IconifyIcon)
                .NotEmpty()
                .WithMessage("İcon Boş geçilemez")
                .MaximumLength(50)
                .WithMessage("50 Karakterden daha fazla yetenek resim linki olamaz");


        }
    }
}
