using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class GithubRepo:BaseEntity
    {
        public string RepoName { get; set; }= string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; }
    }
}
