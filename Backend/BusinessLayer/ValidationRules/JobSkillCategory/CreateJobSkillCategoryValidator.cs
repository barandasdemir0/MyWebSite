using DtoLayer.JobSkillCategoryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.JobSkillCategory;

public class CreateJobSkillCategoryValidator:AbstractValidator<CreateJobSkillCategoryDto>
{
    public CreateJobSkillCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .WithMessage("Lütfen Bu Alanı Boş geçmeyiniz.")
            .MaximumLength(50)
            .WithMessage("Maksimum Girilecek Kategori Adı 50dir.");
        RuleFor(x => x.CategoryDescription)
            .NotEmpty()
            .WithMessage("Lütfen Bu Alanı Boş geçmeyiniz.")
            .MaximumLength(200)
            .WithMessage("Maksimum Girilecek açıklama 200 karakterdir.");
        RuleFor(x => x.CategoryIcon)
            .NotEmpty()
            .WithMessage("Lütfen Bu Alanı Boş geçmeyiniz.")
            .MaximumLength(100)
            .WithMessage("Maksimum Girilecek icon 100 karakterdir.");
    }
}
