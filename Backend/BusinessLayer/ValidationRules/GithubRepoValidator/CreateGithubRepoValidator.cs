using DtoLayer.GithubRepoDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.GithubRepoValidator
{
    public class CreateGithubRepoValidator:AbstractValidator<CreateGithubRepoDto>
    {
        public CreateGithubRepoValidator()
        {
            RuleFor(x => x.RepoName)
                .NotEmpty()
                .WithMessage("Bu Alan boş geçilemez")
                .MaximumLength(200)
                .WithMessage("Lütfen Mantıklı bir uzunlukta kullanıcı adınızı giriniz");
        }
    }
}
