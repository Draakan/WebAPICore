using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUserRegistration.Models
{
    public class Category
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
