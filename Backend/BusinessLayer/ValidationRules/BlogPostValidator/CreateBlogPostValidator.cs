using DataAccessLayer.Abstract;
using DtoLayer.BlogPostDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.BlogPostValidator;

public class CreateBlogPostValidator:AbstractValidator<CreateBlogPostDto>
{
    public CreateBlogPostValidator(ITopicDal topicDal)
    {
        RuleFor(x => x.Title).NotEmpty()
            .WithMessage("Bu Alanı Girmek Zorundasınız")
            .MaximumLength(150)
            .WithMessage("Bu Alan Maksimum 150 Karakter Olmalıdır");


        RuleFor(x => x.CoverImage)
           .MaximumLength(200)
           .WithMessage("Bu Alan Maksimum 200 Karakter Olmalıdır");

        RuleFor(x => x.Technologies)
           .MaximumLength(50)
           .WithMessage("Bu Alan Maksimum 50 Karakter Olmalıdır");

        RuleFor(x => x.Content).NotEmpty()
           .WithMessage("Bu Alanı Girmek Zorundasınız");

        RuleFor(x => x.TopicIds).NotEmpty()
           .WithMessage("Kategori Girilmesi zorunludur");

        RuleForEach(x => x.TopicIds)
            .MustAsync(async (topicId, cancellation) =>
            {
                var exists = await topicDal.GetByIdAsync(topicId);
                return exists != null;
            }).WithMessage("Seçilen Kategori Mevcut değil veya silinmiş");





    }
}
