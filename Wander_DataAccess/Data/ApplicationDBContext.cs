using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wander_Models;

namespace Wander_DataAccess.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        
        {

        }

        public DbSet<Address> Address { get; set; }

        public DbSet<Property>Property { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

   
    }
}
