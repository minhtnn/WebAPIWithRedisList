using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.Data
{
    public class ShipperContext : DbContext
    {
        public ShipperContext(DbContextOptions<ShipperContext> opt) : base(opt) 
        {
            
        }

        #region DbSet
        public DbSet<Shipper> Shippers { get; set; }
        #endregion
    }
}
