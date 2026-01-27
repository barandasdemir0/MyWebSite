using DtoLayer.HeroDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.HeroValidator
{
    public class CreateHeroValidator:AbstractValidator<CreateHeroDto>
    {
        public CreateHeroValidator()
        {
            RuleFor(x => x.FullName).NotEmpty()
                .WithMessage("Lütfen Ad ve Soyadınızı Giriniz")
                .MinimumLength(5)
                .WithMessage("5 Karakterden daha az bir ad ve soyadı girmeyiniz")
                .MaximumLength(50)
                .WithMessage("50 Karakterden fazla olmadığına dikkat ediniz");

            RuleFor(x => x.ScrollingText)
               .MaximumLength(1000)
               .WithMessage("1000 Karakterden fazla olmadığına dikkat ediniz");

            RuleFor(x => x.HeroAbout)
                .NotEmpty()
                .WithMessage("Lütfenhakkımda kısmını Giriniz")
               .MaximumLength(2000)
               .WithMessage("2000 Karakterden fazla olmadığına dikkat ediniz");
        }
    }
}
