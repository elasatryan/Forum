using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.Entities
{
    public class Base
    {
        [Key]
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}