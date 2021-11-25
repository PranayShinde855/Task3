using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Task3.Models;


namespace Task3.Services
{
    public class DBModel : DbContext
    {
        public DBModel() : base("con")
        {
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }

        public System.Data.Entity.DbSet<Task3.Models.Role> Roles { get; set; }

        public System.Data.Entity.DbSet<Task3.Models.User> Users { get; set; }
    }
}