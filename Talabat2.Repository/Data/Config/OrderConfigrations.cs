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
    public class OrderConfigrations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, Sh => Sh.WithOwner()); // one To one (Total)

            builder.Property(o => o.Status).HasConversion( //take 2 Lombede Expression

                //(1) we store it at DB as string
                statuseStore => statuseStore.ToString(),
                //(2) whene we Get It From DB To App We Converte It From String To Enum To Recive It at OrderStatuse Enum
                statuseConvert => (OrderStatus)Enum.Parse(typeof(OrderStatus), statuseConvert)
                );


            builder.Property(o=> o.SubTotal).HasPrecision(18,2);


            builder.HasMany(o => o.Items)
                 .WithOne()
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
