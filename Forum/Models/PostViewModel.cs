using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string ThreadId { get; set; }

        [Required]
        public string Body { get; set; }

        public string ThreadTitle { get; set; }

        public string Nickname { get; set; }

        public bool IsCurrentThreadInactive { get; set; }

        public UserInfo UserInfo { get; set; }

        public DateTime? LastPostDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}