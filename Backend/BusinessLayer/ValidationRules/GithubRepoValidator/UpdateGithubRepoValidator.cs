using DtoLayer.GithubRepoDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.GithubRepoValidator
{
    public class UpdateGithubRepoValidator:AbstractValidator<UpdateGithubRepoDto>
    {
        public UpdateGithubRepoValidator()
        {
            RuleFor(x => x.Id)
           .NotEmpty().WithMessage("ID alanı boş olamaz.");
            RuleFor(x => x.RepoName)
                .NotEmpty().WithMessage("Repo adı boş geçilemez.")
                .MaximumLength(200).WithMessage("Repo adı en fazla 200 karakter olabilir.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
            RuleFor(x => x.RepoUrl)
                 .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.RepoUrl))
                 .WithMessage("Geçerli bir URL giriniz.");
        }
    }
}
