using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Entities
{
    [Table("Threads")]
    public class Thread : Base
    {
        public string UserId { get; set; }

        public string TopicId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Inactive { get; set; }

        public List<Post> Posts { get; set; }

        public Topic Topic { get; set; }
    }
}