using Microsoft.Extensions.DependencyInjection;
using SchedulingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SchedulingApp.Infrastucture.Sql
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<SchedulingAppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ConferenceUser>>();

            context.Database.EnsureCreated();

            if (await userManager.FindByEmailAsync("mrudens@gmail.com") == null)
            {
                var newUser = new ConferenceUser()
                {
                    UserName = "antonsjelchins",
                    Email = "mrudens@gmail.com",
                    FirstEvent = DateTime.UtcNow
                };

                await userManager.CreateAsync(newUser, "q1w2e3r4t5.");
            }
            if (!context.Events.Any())
            {
                var defaultcategories = new List<Category>()
                {
                    new Category() { Name = "Business", Description = "Startups, coaching, financial events and more.", ParentId = Guid.Empty },
                    new Category() { Name = "Music", Description = "Parties, festivals and more.", ParentId = Guid.Empty },
                    new Category() { Name = "Holiday", Description = "Interesting places, events for your free time.", ParentId = Guid.Empty }
                };

                var defaultEvent = new Event()
                {
                    Name = "Atvērto durvju diena LU Datorikas fakultātē",
                    Description = "Visi studētgribētāji ir laipni aicināti piedalīties Datorikas fakultātes Atvērto durvju dienā 2018. gada 19. martā plkst. 12.00 Raiņa bulvārī 19, 12. auditorijā!\r\nViesus uzrunās Datorikas fakultātes dekāns, studiju programmu direktori un studentu pašpārvaldes pārstāvji, būs iespēja uzzināt papildu informāciju par studiju programmām un uzdot jautājumus.",
                    //Categories = defaultcategories,
                    Locations = new List<Location>()
                    {
                        new Location() { Name = "Raiņa bulvāris 19, Centra rajons, Rīga, LV-1050", EventStart = new DateTime(2018, 3, 19, 12, 00, 0), EventEnd = new DateTime(2018, 3, 19, 14, 00, 0), Latitude = 56.9508026,  Longitude = 24.1163189 }
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

                context.Members.AddRange(members);
                context.Events.Add(defaultEvent);
                context.Categories.AddRange(defaultcategories);
                context.SaveChanges();
            }
        }
    }
}
