using Frens.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frens.Payloads;

namespace Frens.Entities
{
    public class FrensContext : DbContext
    {
        public FrensContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<Follow> Followers{get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(c => c.Comment)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

           /* modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(c => c.Comment)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);*/
        }

        }
}
