using DtoLayer.AuthDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ad Alanı Girmek Zorunludur")
            .MaximumLength(50)
            .WithMessage("İsim maksimum 50 karakter olmalıdır");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Ad Alanı Girmek Zorunludur")
            .MaximumLength(50)
            .WithMessage("Soyadı maksimum 50 karakter olmalıdır");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email Alanı Girmek Zorunludur")
            .EmailAddress()
            .WithMessage("Geçerli Bir Email Adresi Giriniz");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Şifre Alanı Girmek Zorunludur")
            .MinimumLength(16)
            .WithMessage("16 Karakterli Bir şifre Giriniz");
    }
}
