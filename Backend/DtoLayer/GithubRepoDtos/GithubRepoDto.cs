using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.GithubRepoDtos;

public class GithubRepoDto
{
    public Guid Id { get; set; }
    public string RepoName { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; }
}
