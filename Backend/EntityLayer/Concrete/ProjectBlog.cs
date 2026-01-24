using System;

namespace CV.EntityLayer.Entities
{
    public sealed class ProjectBlog
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;
    }
}
