using DtoLayer.AuthDtos.Requests;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class ChangePasswordValidator:AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Mevcut şifre zorunludur");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre zorunludur")
            .MinimumLength(16).WithMessage("Yeni şifre en az 16 karakter olmalıdır");
        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Şifre tekrarı zorunludur")
            .Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor");
    }
}
