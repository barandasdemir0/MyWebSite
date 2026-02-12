using DtoLayer.GithubRepoDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.GithubRepoValidator
{
    public class CreateGithubRepoValidator : AbstractValidator<CreateGithubRepoDto>
    {
        public CreateGithubRepoValidator()
        {
            RuleFor(x => x.RepoName)
           .NotEmpty().WithMessage("Repo adı boş geçilemez.")
           .MaximumLength(200).WithMessage("Repo adı en fazla 200 karakter olabilir.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
            RuleFor(x => x.Language)
                .MaximumLength(100).WithMessage("Dil alanı en fazla 100 karakter olabilir.");
            RuleFor(x => x.RepoUrl)
                .NotEmpty().WithMessage("Repo URL boş olamaz.") // Url zorunlu olsun dedik
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.RepoUrl))
                .WithMessage("Geçerli bir URL giriniz.");
            RuleFor(x => x.StarCount)
                .GreaterThanOrEqualTo(0).WithMessage("Yıldız sayısı 0'dan küçük olamaz.");
            RuleFor(x => x.ForkCount)
                .GreaterThanOrEqualTo(0).WithMessage("Fork sayısı 0'dan küçük olamaz.");
        }
    }
}

