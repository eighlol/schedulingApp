using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SchedulingApp.Models
{
    public class SchedulingAppDbContextSeedData
    {
        private readonly SchedulingAppDbContext _context;
        private readonly UserManager<ConferenceUser> _userManager;

        public SchedulingAppDbContextSeedData(SchedulingAppDbContext context, UserManager<ConferenceUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task EnsureSeedDataAsync()
        {
            if (await _userManager.FindByEmailAsync("mrudens@gmail.com") == null)
            {
                var newUser = new ConferenceUser()
                {
                    UserName = "antonsjelchins",
                    Email = "mrudens@gmail.com",
                    FirstEvent = DateTime.UtcNow
                };

                await _userManager.CreateAsync(newUser, "q1w2e3r4t5.");
            }
            if (!_context.Events.Any())
            {
                var defaultcategories = new List<Category>()
                {
                    new Category() { Name = "Business", Description = "Startups, coaching, financial events and more.", ParentId = Guid.Empty },
                    new Category() { Name = "Music", Description = "Parties, festivals and more.", ParentId = Guid.Empty },
                    new Category() { Name = "Holiday", Description = "Interesting places, events for your free time.", ParentId = Guid.Empty }
                };

                var defaultEvent = new Event()
                {
                    Name = "RVT sapulce",
                    Description = "Learning is a new adventure!",
                    //Categories = defaultcategories,
                    Locations = new List<Location>()
                    {
                        new Location() { Name = "Krišjāņa Valdemāra iela 1C,Rīga, LV-1010", EventStart = new DateTime(2016, 6, 29, 7, 30, 0), EventEnd = new DateTime(2016, 6, 29, 9, 30, 0), Latitude = 56.95288888888889,  Longitude = 24.10377777777778 }
                    },
                    UserName = "antonsjelchins"
                };
                var members = new List<Member>()
                {
                    new Member() {Name ="Daniels Kudrins", Gender = "male" },
                    new Member() {Name ="Andrejs Ivanovs", Gender = "male" },
                    new Member() {Name ="Dmitrijs Kuzenkovs", Gender = "male" },
                    new Member() {Name = "Aleksandra Kustova", Gender= "female" },
                    new Member() {Name ="Vladislavs Ķemers", Gender = "male" },
                    new Member() {Name = "Anastasija Elksnite", Gender= "female" }
                };

                _context.Members.AddRange(members);
                _context.Events.Add(defaultEvent);
                _context.Categories.AddRange(defaultcategories);
                _context.SaveChanges();
            }

        }
    }
}
