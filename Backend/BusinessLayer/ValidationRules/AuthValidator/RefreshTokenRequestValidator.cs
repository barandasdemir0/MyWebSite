using DtoLayer.AuthDtos;
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
//Biri Postman veya curl ile API'ne direkt istek atarsa boş body gönderebilir. Validator olmadan AuthManager.RefreshTokenAsync içinde 

//GetPrincipalFromExpiredToken(null)
// çağrılır → exception fırlar.

//Yani: Bu validator kullanıcı için değil, API'ni koruması için.