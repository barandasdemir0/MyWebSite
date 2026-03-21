using DtoLayer.AuthDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class ForgotPasswordValidator:AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
          .NotEmpty().WithMessage("E-posta alanı zorunludur")
          .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin");
    }
}
