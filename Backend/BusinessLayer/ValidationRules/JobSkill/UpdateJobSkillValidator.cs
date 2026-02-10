using DtoLayer.JobSkillsDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.JobSkill;

public class UpdateJobSkillValidator:AbstractValidator<UpdateJobSkillDto>
{
    public UpdateJobSkillValidator()
    {
        RuleFor(x => x.JobSkillName).NotEmpty()
           .WithMessage("Lütfen Boş Geçmeyiniz")
           .MaximumLength(50)
           .WithMessage("50 Karakterden fazla olamaz");

        RuleFor(x => x.JobSkillPercentange)
     .NotNull().WithMessage("Lütfen boş geçmeyiniz")
     .InclusiveBetween(0, 100).WithMessage("0 ile 100 arasında bir değer giriniz");
    }
}
