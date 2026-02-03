using DataAccessLayer.Abstract;
using DtoLayer.BlogpostDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.BlogPostValidator
{
    public class UpdateBlogPostValidator:AbstractValidator<UpdateBlogPostDto>
    {
        public UpdateBlogPostValidator(ITopicDal topicDal)
        {
            RuleFor(x => x.Title).NotEmpty()
              .WithMessage("Bu Alanı Girmek Zorundasınız")
              .MaximumLength(150)
              .WithMessage("Bu Alan Maksimum 150 Karakter Olmalıdır");


            RuleFor(x => x.CoverImage)
               .MaximumLength(200)
               .WithMessage("Bu Alan Maksimum 200 Karakter Olmalıdır");

            RuleFor(x => x.Title).NotEmpty()
               .WithMessage("Bu Alanı Girmek Zorundasınız");

            RuleFor(x => x.TopicIds).NotEmpty()
                .WithMessage("Kategori Girilmesi zorunludur");

            RuleForEach(x => x.TopicIds).MustAsync(async (topicId, cancelation) =>
            {
                var exists = await topicDal.GetByIdAsync(topicId);
                return exists != null;
            }).WithMessage("Seçilen Kategori Mevcut değil veya silinmiş");

        }
    }
}
