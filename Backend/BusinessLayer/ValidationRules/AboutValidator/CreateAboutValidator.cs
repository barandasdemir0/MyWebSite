using DtoLayer.AboutDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.AboutValidator
{
    public class CreateAboutValidator : AbstractValidator<CreateAboutDto>
    {
        public CreateAboutValidator()
        {
            RuleFor(x => x.FullName).NotEmpty()
                .WithMessage("İsim Alanı Girmek Zorunlusunuz")
                .MinimumLength(5)
                .WithMessage("İsminiz 5 karakterden az olamaz")
                .MaximumLength(50)
                .WithMessage("İsminiz 50 Karakterden fazla olamaz");

            RuleFor(x => x.Greeting).NotEmpty()
                .WithMessage("Bu Alan Boş Bırakılamaz")
                .MaximumLength(20)
                .WithMessage("Bu alan 20 karakterden fazla olamaz");

            RuleFor(x => x.Bio).NotEmpty()
                .WithMessage("Bu Alan Boş Bırakılamaz")
                .MinimumLength(20)
                .WithMessage("En az 20 karakterde bir yazı yazınız")
                .MaximumLength(2000)
                .WithMessage("En fazla 2000 karakterde yazı yazabilirsiniz");


            RuleFor(x => x.ProfileImage)
                .NotNull()
                .WithMessage("Resim Yüklemek Zorunludur");

            RuleFor(x => x.ProjectCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Proje Sayısı 0 dan küçük olamaz");

            RuleFor(x => x.ProjectDrink)
                .GreaterThanOrEqualTo(0)
                .WithMessage("İçilen İçecek Sayısı 0 dan küçük olamaz");

            RuleFor(x => x.ExperienceYear)
                .InclusiveBetween(0, 50)
                .WithMessage("Tecrübe 0 ile 50 yıl arasına olmalıdır");
        }

    }
}
