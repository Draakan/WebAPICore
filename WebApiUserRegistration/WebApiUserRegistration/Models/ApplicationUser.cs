using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUserRegistration.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Categories = new List<Category>();
            Events = new List<Event>();
        }

        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }
        
        public virtual Bill UsersBill { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
