using System;
using System.Collections.Generic;

namespace CV.EntityLayer.Entities
{
    public sealed class BlogPost : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public int ReadTime { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime? PublishedAt { get; set; }
        public int ViewCount { get; set; } = 0;

        //// Many-to-Many Navigation
        public ICollection<BlogTopic> BlogTopics { get; set; } = new List<BlogTopic>();
        public ICollection<ProjectBlog> ProjectBlogs { get; set; } = new List<ProjectBlog>();
    }
}
