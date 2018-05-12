using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conference.Models
{
    public class ConferenceDbContextSeedData
    {
        private ConferenceDbContext _context;
        private UserManager<ConferenceUser> _userManager;

        public ConferenceDbContextSeedData(ConferenceDbContext context, UserManager<ConferenceUser> userManager)
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

                var test = await _userManager.CreateAsync(newUser, "q1w2e3r4t5");
            }
            if (!_context.Events.Any())
            {
                var defaultcategories = new List<Category>()
                {
                    new Category() { Name = "Business", Description = "Startups, coaching, financial events and more.", ParentId = 0 },
                    new Category() { Name = "Music", Description = "Parties, festivals and more.", ParentId = 0 },
                    new Category() { Name = "Holiday", Description = "Interesting places, events for your free time.", ParentId = 0 }
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
                    new Member() {Name ="Daniels Kudrins", Sex = "male" },
                    new Member() {Name ="Andrejs Ivanovs", Sex = "male" },
                    new Member() {Name ="Dmitrijs Kuzenkovs", Sex = "male" },
                    new Member() {Name = "Aleksandra Kustova", Sex= "female" },
                    new Member() {Name ="Vladislavs Ķemers", Sex = "male" },
                    new Member() {Name = "Anastasija Elksnite", Sex= "female" }
                };

                _context.Members.AddRange(members);
                _context.Events.Add(defaultEvent);
                _context.Categories.AddRange(defaultcategories);
                _context.SaveChanges();
            }

        }
    }
}
