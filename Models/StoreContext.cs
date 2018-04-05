using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
 
namespace RobotJester.Models
{
    public class StoreContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
        public DbSet<Customers> customers { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Addresses> addresses { get; set; }
        public DbSet<Reviews> reviews { get; set; }
    }
}