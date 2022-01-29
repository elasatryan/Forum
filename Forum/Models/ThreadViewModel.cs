using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class ThreadViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string TopicId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Inactive { get; set; }

        public string Nickname { get; set; }

        public string TopicTitle { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public List<PostViewModel> Posts { get; set; }
    }
}