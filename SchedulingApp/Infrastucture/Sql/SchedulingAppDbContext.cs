using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.Infrastucture.Sql
{
    public class SchedulingAppDbContext : IdentityDbContext<ScheduleAppUser>
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<EventMember> EventMembers { get; set; }

        public DbSet<EventCategory> EventCategories { get; set; }

        public DbSet<Member> Members { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<EventCategory>().HasKey(x => new { x.EventId, x.CategoryId });
            builder.Entity<EventMember>().HasKey(x => new { x.EventId, x.MemberId });

            builder.Entity<Location>()
                .HasOne(l => l.Event)
                .WithMany(e => e.Locations)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public SchedulingAppDbContext(DbContextOptions<SchedulingAppDbContext> options) : base(options) { }
    }
}
