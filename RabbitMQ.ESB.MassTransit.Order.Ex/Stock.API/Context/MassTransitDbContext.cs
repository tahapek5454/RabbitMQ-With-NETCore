using Microsoft.EntityFrameworkCore;

namespace Stock.API.Context
{
    public class MassTransitDbContext : DbContext
    {
        public MassTransitDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Stock> Stocks { get; set; }
    }
}
