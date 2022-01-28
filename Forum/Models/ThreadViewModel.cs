using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models
{
    public class ThreadViewModel
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Inactive { get; set; }

        public string Nickname { get; set; }

        public List<PostViewModel> Posts { get; set; }
    }
}