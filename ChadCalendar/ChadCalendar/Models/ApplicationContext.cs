using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Project> Projects { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public ApplicationContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChadCalendar");
            //System.IO.Directory.CreateDirectory(path);
            //optionsBuilder.UseSqlite($"Data Source = { Path.Combine(path,"Calendar.db")}");
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
