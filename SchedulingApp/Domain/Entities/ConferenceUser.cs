using System;
using Microsoft.AspNetCore.Identity;

namespace SchedulingApp.Domain.Entities
{
    public class ConferenceUser : IdentityUser
    {
        public DateTime FirstEvent { get; set; }

        //public ConferenceUser(string name) : base(name) { }

    }
}