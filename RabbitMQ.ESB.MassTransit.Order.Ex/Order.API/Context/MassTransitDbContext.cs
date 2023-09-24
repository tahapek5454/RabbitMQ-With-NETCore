using Microsoft.EntityFrameworkCore;

namespace Order.API.Context
{
    public class MassTransitDbContext: DbContext
    {
        public MassTransitDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.OrderItem> OrderItems { get; set; }


    }
}
