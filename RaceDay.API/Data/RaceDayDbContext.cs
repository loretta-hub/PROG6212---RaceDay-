
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(
            DbContextOptions<RaceDayDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventType> EventTypes => Set<EventType>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Enrolment> Enrolments => Set<Enrolment>();
        public DbSet<Result> Results => Set<Result>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<EventType>().ToTable("EventType");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Enrolment>().ToTable("Enrolment");
            modelBuilder.Entity<Result>().ToTable("Result");

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<EventType>()
                .HasIndex(e => e.TypeName)
                .IsUnique();

            modelBuilder.Entity<Event>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne<EventType>()
                .WithMany()
                .HasForeignKey(e => e.EventTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne<Event>()
                .WithMany()
                .HasForeignKey(c => c.EventsID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.ParticipantID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()
                .HasOne<Event>()
                .WithMany()
                .HasForeignKey(e => e.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()
                .HasIndex(e => new { e.ParticipantID, e.EventID })
                .IsUnique();

            modelBuilder.Entity<Result>()
                .HasOne<Enrolment>()
                .WithMany()
                .HasForeignKey(r => r.EnrollmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Result>()
                .HasIndex(r => r.EnrollmentID)
                .IsUnique();
        }
    }
}
