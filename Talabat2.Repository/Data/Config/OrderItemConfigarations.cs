using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities.Order_Aggregate;

namespace Talabat2.Repository.Data.Config
{
    public class OrderItemConfigarations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.OwnsOne(om => om.Product, prod => prod.WithOwner());

            builder.Property(om => om.Price).HasPrecision(18, 2);


        }
    }
}
