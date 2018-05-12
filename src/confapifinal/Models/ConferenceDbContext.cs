using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace Conference.Models
{
    public class ConferenceDbContext : IdentityDbContext<ConferenceUser>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<EventMember> EventMembers { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Member> Members { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:ConferenceContextConnection"];
            optionsBuilder.UseSqlServer(connString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 
            builder.Entity<EventCategory>().HasKey(x => new { x.EventId, x.CategoryId });
            builder.Entity<EventMember>().HasKey(x => new { x.EventId, x.MemberId });
        }

        public ConferenceDbContext()
        {
            Database.EnsureCreated();
        }
    }
}
