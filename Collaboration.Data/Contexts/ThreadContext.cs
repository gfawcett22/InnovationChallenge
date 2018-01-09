using System;
using Microsoft.EntityFrameworkCore;
using Collaboration.Core.Models;

namespace Collaboration.Data.Contexts
{
    public class ThreadContext : DbContext
    {
        public ThreadContext(DbContextOptions<ThreadContext> options) : base(options) { }

        public DbSet<Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
