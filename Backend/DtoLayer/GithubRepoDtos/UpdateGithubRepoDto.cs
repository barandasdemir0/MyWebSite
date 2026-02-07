using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.GithubRepoDtos;

public class UpdateGithubRepoDto
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string RepoName { get; set; } = string.Empty;
    public int? DisplayOrder { get; set; }
    public bool IsVisible { get; set; }
}
