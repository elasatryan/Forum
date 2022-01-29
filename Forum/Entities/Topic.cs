using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Entities
{
    [Table("Topics")]
    public class Topic : Base
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public List<Thread> Threads { get; set; }
    }
}