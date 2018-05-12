using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Conference.Models
{
    public class ConferenceUser : IdentityUser
    {
        public DateTime FirstEvent { get; set; }

        //public ConferenceUser(string name) : base(name) { }

    }
}