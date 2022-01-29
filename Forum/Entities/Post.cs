using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Entities
{
    [Table("Posts")]
    public class Post : Base
    {
        public string UserId { get; set; }

        public string ThreadId { get; set; }

        public string Body { get; set; }
    }
}