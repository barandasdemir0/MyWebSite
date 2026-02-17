using DtoLayer.TopicDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.TopicValidator;

public class UpdateTopicValidator:AbstractValidator<UpdateTopicDto>
{
    public UpdateTopicValidator()
    {
        RuleFor(x => x.Name)
          .NotEmpty()
          .WithMessage("Lütfen Bu alanı Boş bırakmayınız")
          .MaximumLength(100)
          .WithMessage("Lütfen 100 Karakterden daha fazla bir kategori ismi girmeyiniz");
    }
}
