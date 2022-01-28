using System.Data.Entity;

using Forum.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Models
{
    public class ForumContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Thread> Threads { get; set; }

        public DbSet<Post> Posts { get; set; }

        public ForumContext()
            : base("DefaultConnection")
        {
        }

        public static ForumContext Create()
        {
            return new ForumContext();
        }
    }
}