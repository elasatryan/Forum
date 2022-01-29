using System.Data.Entity;

using Forum.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Models
{
    public class ForumContext: IdentityDbContext<ApplicationUser>
    {
        #region private members

        private static ForumContext _forumContext = null;

        private ForumContext()
            : base("DefaultConnection")
        {
        }

        #endregion private members

        #region public members

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Thread> Threads { get; set; }

        public DbSet<Post> Posts { get; set; }

        public static ForumContext Create()
        {
            return new ForumContext();
        }
        #endregion public members

    }
}