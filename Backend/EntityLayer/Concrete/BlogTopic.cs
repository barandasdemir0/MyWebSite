using System;

namespace CV.EntityLayer.Entities
{
    public sealed class BlogTopic
    {
        //Veritabanlarında doğrudan Many-to-Many ilişki kurulamaz. Bu yüzden bir ara tablo (Join Table / Junction Table) gerekir:


        public Guid BlogPostId { get; set; } // FK → BlogPost tablosuna
        public BlogPost BlogPost { get; set; } = null!; // Navigation property

        public Guid TopicId { get; set; }  // FK → Topic tablosuna
        public Topic Topic { get; set; } = null!;   // Navigation property
    }
}
