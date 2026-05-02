using DtoLayer.JobSkillCategoryDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.JobSkillCategory;

public class UpdateJobSkillCategoryValidator:AbstractValidator<UpdateJobSkillCategoryDto>
{
    public UpdateJobSkillCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
           .NotEmpty()
           .WithMessage("Lütfen Bu Alanı Boş geçmeyiniz.")
           .MaximumLength(50)
           .WithMessage("Maksimum Girilecek Kategori Adı 50dir.").MustBeSafeHtml();
        RuleFor(x => x.CategoryDescription)
            .NotEmpty()
            .WithMessage("Lütfen Bu Alanı Boş geçmeyiniz.")
            .MaximumLength(200)
            .WithMessage("Maksimum Girilecek açıklama 200 karakterdir.").MustBeSafeHtml();
        RuleFor(x => x.CategoryIcon)
            .NotEmpty()
            .WithMessage("Lütfen Bu Alanı Boş geçmeyiniz.")
            .MaximumLength(100)
            .WithMessage("Maksimum Girilecek icon 100 karakterdir.");
    }
}
