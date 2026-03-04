using DtoLayer.AuthDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class LoginValidator:AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
         .NotEmpty().WithMessage("E-posta alanı zorunludur")
         .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı zorunludur");
    }
}
