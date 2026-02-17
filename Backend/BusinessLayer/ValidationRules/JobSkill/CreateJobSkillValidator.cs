using DtoLayer.JobSkillsDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.JobSkill;

public class CreateJobSkillValidator:AbstractValidator<CreateJobSkillDto>
{
    public CreateJobSkillValidator()
    {
        RuleFor(x => x.JobSkillName).NotEmpty()
            .WithMessage("Lütfen Boş Geçmeyiniz")
            .MaximumLength(50)
            .WithMessage("50 Karakterden fazla olamaz");

        RuleFor(x => x.JobSkillPercentage)
     .NotNull().WithMessage("Lütfen boş geçmeyiniz")
     .InclusiveBetween(0, 100).WithMessage("0 ile 100 arasında bir değer giriniz");


    }
}
