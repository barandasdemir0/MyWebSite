using System;

namespace CV.EntityLayer.Entities
{
    public sealed class BlogTopic
    {
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;

        public Guid TopicId { get; set; }
        public Topic Topic { get; set; } = null!;
    }
}
