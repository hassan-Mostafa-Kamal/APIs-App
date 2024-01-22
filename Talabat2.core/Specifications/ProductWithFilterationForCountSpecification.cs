using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.Specifications;
using Talabat2.core.Entities;

namespace Talabat2.core.Specifications
{
    public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
    {

        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams) :
             base(
               // Constructor Parmeters for critaria 
               p =>
                  (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
                 (!specParams.BrandId.HasValue || p.ProductBrandId == specParams.BrandId.Value) &&
                  (!specParams.TypeId.HasValue || p.ProductTypeId == specParams.TypeId.Value)
               )

        {

        }
    }
}
