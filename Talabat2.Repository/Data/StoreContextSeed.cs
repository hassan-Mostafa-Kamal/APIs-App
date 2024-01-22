using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat2.core.Entities;
using Talabat2.core.Entities.Order_Aggregate;

namespace Talabat2.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbcontext)
        {
            var brandData = File.ReadAllText("../Talabat2.Repository/Data/DataSeed/brands.json");

            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

            var brandsWithoutId = brands.Select(b => new ProductBrand
            {
                Name = b.Name
            });
            if (!dbcontext.productBrands.Any())
            {
                if (brands != null && brands.Count > 0)
                {
                    foreach (var brand in brandsWithoutId)
                    {

                        await dbcontext.Set<ProductBrand>().AddAsync(brand);

                    }

                     await dbcontext.SaveChangesAsync();


                }
            }

            //===============================================================================//

            var typesDate = File.ReadAllText("../Talabat2.Repository/Data/DataSeed/types.json");

            var types = JsonSerializer.Deserialize<List<ProductType>>(typesDate);

            var typesWithoutId = types.Select(t => new ProductType
            {
                Name = t.Name
            });
            if (!dbcontext.productTypes.Any())
            {
                if (types != null && types.Count > 0)
                {
                    foreach (var type in typesWithoutId)
                    {

                        await dbcontext.Set<ProductType>().AddAsync(type);

                    }

                    await dbcontext.SaveChangesAsync();


                }
            }

            //==========================================================================================//

            var prodectData = File.ReadAllText("../Talabat2.Repository/Data/DataSeed/products.json");

            var products = JsonSerializer.Deserialize<List<Product>>(prodectData);

            var productWithoutId = products.Select(p => new Product
            {
                Name = p.Name,
                Description = p.Description,           
                ProductBrandId = p.ProductBrandId,
                ProductTypeId = p.ProductTypeId,
                PictureUrl = p.PictureUrl,
                Price= p.Price

            });
            if (!dbcontext.Prodects.Any())
            {
                if (products != null && products.Count > 0)
                {
                    foreach (var product in productWithoutId)
                    {

                        await dbcontext.Set<Product>().AddAsync(product);

                    }

                    await dbcontext.SaveChangesAsync();


                }
            }

            //============================================================
            var deliveryMethodData = File.ReadAllText("../Talabat2.Repository/Data/DataSeed/delivery.json");

            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

            var deliveryMethodWthoutId = deliveryMethods.Select(d => new DeliveryMethod
            {
                ShortName = d.ShortName,
                Description = d.Description,
                Cost = d.Cost,
                DeliveryTime = d.DeliveryTime,

            });
            if (!dbcontext.DeliveryMethods.Any())
            {
                if (deliveryMethods != null && deliveryMethods.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethodWthoutId)
                    {

                        await dbcontext.Set<DeliveryMethod>().AddAsync(deliveryMethod);

                    }

                    await dbcontext.SaveChangesAsync();


                }
            }



        }

    }
}
