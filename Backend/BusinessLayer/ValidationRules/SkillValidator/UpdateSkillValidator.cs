using DtoLayer.SkillDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.SkillValidator
{
    public class UpdateSkillValidator:AbstractValidator<UpdateSkillDto>
    {
        public UpdateSkillValidator()
        {
            RuleFor(x => x.SkillName)
               .NotEmpty()
               .WithMessage("Yetenekleriniz boş geçilemez")
               .MaximumLength(100)
               .WithMessage("100 Karakterden daha fazla yetenek ismi olamaz");

            RuleFor(x => x.SkillUrl)
                .MaximumLength(200)
                .WithMessage("200 Karakterden daha fazla yetenek resim linki olamaz");

            RuleFor(x => x.SkillIcon)
                .MaximumLength(200)
                .WithMessage("200 Karakterden daha fazla yetenek iconu linki olamaz");
        }
    }
}
