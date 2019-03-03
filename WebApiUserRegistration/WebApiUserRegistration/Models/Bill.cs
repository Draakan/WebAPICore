using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUserRegistration.Models
{
    public class Bill
    {
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public int Value { get; set; }
        public string Currency { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
