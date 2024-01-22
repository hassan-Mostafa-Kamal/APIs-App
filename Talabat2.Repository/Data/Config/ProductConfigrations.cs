using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities;

namespace Talabat2.Repository.Data.Config
{
    internal class ProductConfigrations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.ProdectBrand).WithMany().HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(t=> t.ProductType).WithMany().HasForeignKey(p=> p.ProductTypeId);


            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p=> p.PictureUrl).IsRequired();

            builder.Property(p => p.Price).HasPrecision(18, 2);

        }
    }
}
