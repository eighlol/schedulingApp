using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conference.Models
{
    public class Duration
    {
        public DateTime From { get; set; }
        public DateTime Till { get; set; }

        public TimeSpan Difference {
            get
            {
                return (From - Till);
            }
        }

        public Duration()
        {

        }
    }
}
