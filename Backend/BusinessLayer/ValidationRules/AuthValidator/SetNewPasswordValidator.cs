using DtoLayer.AuthDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class SetNewPasswordValidator:AbstractValidator<SetNewPasswordDto>
{
    public SetNewPasswordValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage("E-posta zorunludur")
           .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre zorunludur")
            .MinimumLength(16).WithMessage("Şifre en az 16 karakter olmalıdır");
    }
}
