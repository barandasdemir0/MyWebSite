using DtoLayer.AuthDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class VerifyResetOtpValidator:AbstractValidator<VerifyResetOtpDto>
{
    public VerifyResetOtpValidator()
    {
        RuleFor(x => x.Email)
          .NotEmpty().WithMessage("E-posta zorunludur")
          .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Doğrulama kodu zorunludur")
            .Length(6).WithMessage("Kod 6 haneli olmalıdır")
            .Matches("^[0-9]+$").WithMessage("Kod sadece rakamlardan oluşmalıdır");
    }
}
