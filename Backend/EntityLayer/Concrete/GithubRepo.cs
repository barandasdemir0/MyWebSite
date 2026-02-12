using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class GithubRepo:BaseEntity
    {
        public string RepoName { get; set; }= string.Empty;
        public string? Description { get; set; }          // Repo açıklaması
        public string? Language { get; set; }              // "C#", "Python", "JavaScript"
        public int StarCount { get; set; }                 // ⭐ sayısı
        public int ForkCount { get; set; }                 // 🍴 sayısı
        public string? RepoUrl { get; set; }               // GitHub linki
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; }
    }
}
