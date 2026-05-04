using DtoLayer.GuestBookDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.GuestBookValidator;

public class CreateGuestBookValidator : AbstractValidator<CreateGuestBookDto>
{
    public CreateGuestBookValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Mesaj Girmeniz Zorunludur")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Konu Alanı Sadece Boşluklardan oluşamaz");
    }
}
