using DtoLayer.TopicDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.TopicValidator
{
    public class CreateTopicValidator:AbstractValidator<CreateTopicDto>
    {
        public CreateTopicValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen Bu alanı Boş bırakmayınız")
                .MaximumLength(100)
                .WithMessage("Lütfen 100 Karakterden daha fazla bir kategori ismi girmeyiniz");
        }
    }
}
