using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<User> Duties { get; set; }
        public DbSet<User> Tasks { get; set; }
        public DbSet<User> Events { get; set; }
        public DbSet<User> Projects { get; set; }
        public ApplicationContext()
        {
          //  Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=\Calendar.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne(s => s.Predecessor)
                .WithOne(p => p.Successor)
                .HasForeignKey<Task>(b => b.SuccessorFK);

            modelBuilder.Entity<Task>()
                .HasOne(p => p.Successor)
                .WithOne(s => s.Predecessor)
                .HasForeignKey<Task>(b => b.PredecessorFK);
        }
    }
}
