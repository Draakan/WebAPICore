using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUserRegistration.Models
{
    public class Event
    {
        public string Id { get; set; }

        public string Type { get; set; }
        public int Amount { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

        public string CategoryId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
