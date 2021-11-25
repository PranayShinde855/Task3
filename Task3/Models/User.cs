using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task3.Models
{
    public class User 
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name ="UserName")]
        public String UserName { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]

        public virtual Role Role { get; set; }

        public string Password { get; set; }

        public static implicit operator int(User v)
        {
            throw new NotImplementedException();
        }
    }
}