using DtoLayer.SocialMediaDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.SocialMediaValidator;

public class UpdateSocialMediaValidator:AbstractValidator<UpdateSocialMediaDto>
{
    public UpdateSocialMediaValidator()
    {
        RuleFor(x => x.SocialMediaName)
           .NotEmpty()
           .WithMessage("Lütfen bu alanı boş bırakmayın")
           .MaximumLength(100)
           .WithMessage("100 karakterden uzun sosyal medya ismi olamaz");

        RuleFor(x => x.SocialMediaUrl)
            .NotEmpty()
            .WithMessage("Lütfen bu alanı boş bırakmayın")
            .MaximumLength(250)
            .WithMessage("250 karakterden uzun sosyal medya urli olamaz");

        RuleFor(x => x.SocialMediaName)
            .NotEmpty()
            .WithMessage("Lütfen bu alanı boş bırakmayın")
            .MaximumLength(100)
            .WithMessage("100 karakterden uzun sosyal iconu olamaz");
    }
}
