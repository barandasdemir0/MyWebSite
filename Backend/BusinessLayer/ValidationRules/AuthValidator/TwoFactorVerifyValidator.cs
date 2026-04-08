using DtoLayer.AuthDtos.Requests;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class TwoFactorVerifyValidator:AbstractValidator<TwoFactorVerifyDto>
{
    public TwoFactorVerifyValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("Kullanıcı kimliği gerekli");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Doğrulama kodu zorunludur")
            .Length(6).WithMessage("Kod 6 haneli olmalıdır")
            .Matches("^[0-9]+$").WithMessage("Kod sadece rakamlardan oluşmalıdır");
    }
}
