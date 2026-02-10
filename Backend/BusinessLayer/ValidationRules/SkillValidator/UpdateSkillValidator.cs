using DtoLayer.SkillDtos;
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
        }
    }
}
