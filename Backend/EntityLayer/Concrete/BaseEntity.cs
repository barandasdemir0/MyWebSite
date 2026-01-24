using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
               Id = Guid.CreateVersion7(); 
        }
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Soft Delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }  // Ne zaman silindi?

    }
}
