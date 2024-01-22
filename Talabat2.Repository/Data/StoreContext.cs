using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities;
using Talabat2.core.Entities.Order_Aggregate;

namespace Talabat2.Repository.Data
{
    public class StoreContext:DbContext
    {

        public StoreContext(DbContextOptions<StoreContext> options):base(options) 
        {


                
        }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        public DbSet<Product> Prodects { get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        public DbSet<ProductBrand> productBrands { get; set; }

        public DbSet<Order>  Orders { get; set; }
        public DbSet<OrderItem>  OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    }
}
