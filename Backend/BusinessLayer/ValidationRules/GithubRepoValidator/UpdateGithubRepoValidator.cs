using DtoLayer.GithubRepoDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.GithubRepoValidator;

public class UpdateGithubRepoValidator:AbstractValidator<UpdateGithubRepoDto>
{
    public UpdateGithubRepoValidator()
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


    }
}
