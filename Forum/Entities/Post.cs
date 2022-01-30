using Forum.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Entities
{
    [Table("Posts")]
    public class Post : Base
    {
        public string UserId { get; set; }

        public string ThreadId { get; set; }

        public string Body { get; set; }

        public Thread Thread { get; set; }

        public ApplicationUser User { get; set; }

    }
}