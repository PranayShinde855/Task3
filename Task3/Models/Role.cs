using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Task3.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Display(Name = "Role")]
        public String Name { get; set; }
        
    }
}