using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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