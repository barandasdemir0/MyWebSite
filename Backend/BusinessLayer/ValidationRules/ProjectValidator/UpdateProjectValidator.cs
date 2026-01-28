using DataAccessLayer.Abstract;
using DtoLayer.ProjectDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.ProjectValidator
{
    public class UpdateProjectValidator:AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectValidator(ITopicDal topicDal)
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Başlık Boş Geçilemez")
                .MaximumLength(200)
                .WithMessage("200 Karakterden daha fazla bir başlık olamaz");

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage("Kısa Açıklama Boş Geçilemez")
                .MaximumLength(1000)
                .WithMessage("1000 Karakterden daha fazla bir Kısa Açıklama olamaz");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Açıklama Boş Geçilemez");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(200)
                .WithMessage("200 Karakterden daha fazla bir resim linki olamaz olamaz");

            RuleFor(x => x.ClientName)
                .MaximumLength(200)
                .WithMessage("200 Karakterden daha fazla bir Müşteri ismi olamaz");

            RuleFor(x => x.Duration)
                .MaximumLength(50)
                .WithMessage("50 Karakterden daha fazla bir zamanı olamaz");

            RuleFor(x => x.Role)
                .MaximumLength(100)
                .WithMessage("100 Karakterden daha fazla bir ekip sayısı olamaz");

            RuleFor(x => x.Goals)
                .MaximumLength(3000)
                .WithMessage("3000 Karakterden daha fazla bir bu iş neden yapıldı olamaz");

            RuleFor(x => x.Features)
                .MaximumLength(3000)
                .WithMessage("3000 Karakterden daha fazla bir özellikleri konusu olamaz");

            RuleFor(x => x.Results)
                .MaximumLength(3000)
                .WithMessage("3000 Karakterden daha fazla bir sonuç çıktısı olamaz");

            RuleFor(x => x.WebsiteUrl)
                .MaximumLength(300)
                .WithMessage("300 Karakterden daha fazla bir web site linki olamaz");

            RuleFor(x => x.GithubUrl)
                .MaximumLength(300)
                .WithMessage("300 Karakterden daha fazla bir github linki olamaz");

            RuleFor(x => x.TopicIds).NotEmpty()
              .WithMessage("Kategori Girilmesi zorunludur");


            RuleForEach(x=>x.TopicIds)
                .MustAsync(async (topic, cancelation) =>
                {
                    var query = await topicDal.GetByIdAsync(topic);
                    return query != null;
                }).WithMessage("Seçilen Kategori Mevcut değil veya silinmiş");

        }
    }
}
