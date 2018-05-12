﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conference.ViewModels
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [Required]
        public DateTime EventStart { get; set; }
        
        [Required]
        public DateTime EventEnd { get; set; }
    }
}
