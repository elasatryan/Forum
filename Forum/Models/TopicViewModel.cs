using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class TopicViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Nickname { get; set; }

        public DateTime? LastPost { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}