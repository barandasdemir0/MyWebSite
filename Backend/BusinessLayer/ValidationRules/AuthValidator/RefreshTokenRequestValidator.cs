using DtoLayer.AuthDtos.Items;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class RefreshTokenRequestValidator:AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.AccessToken)
          .NotEmpty().WithMessage("Access token zorunludur");
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token zorunludur");
    }
}
