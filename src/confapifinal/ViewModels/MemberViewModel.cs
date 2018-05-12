using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conference.ViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }       
        public string Sex { get; set; }
    }
}
