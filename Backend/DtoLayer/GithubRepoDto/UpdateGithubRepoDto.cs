using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.GithubRepoDto
{
    public class UpdateGithubRepoDto
    {
        public Guid Id { get; set; }
        public string RepoName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; }
    }
}
