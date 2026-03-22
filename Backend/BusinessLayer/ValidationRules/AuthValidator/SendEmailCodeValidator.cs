using DtoLayer.AuthDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class SendEmailCodeValidator:AbstractValidator<SendEmailCodeDto>
{
    public SendEmailCodeValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı kimliği zorunludur");
    }
}
