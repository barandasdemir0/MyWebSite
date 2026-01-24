using System;
using System.Collections.Generic;

namespace CV.EntityLayer.Entities
{
    public sealed class Topic : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }

        // Many-to-Many Navigation
        public ICollection<ProjectTopic> ProjectTopics { get; set; } = new List<ProjectTopic>();
        public ICollection<BlogTopic> BlogTopics { get; set; } = new List<BlogTopic>();
    }
}
