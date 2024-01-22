using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.Specifications;
using Talabat2.core.Entities;

namespace Talabat2.core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {

        public ProductWithBrandAndTypeSpecification(ProductSpecParams specParams) :
            base (
                                    // Constructor Parmeters for Critaria
               p=>
               (string.IsNullOrEmpty(specParams.Search)|| p.Name.ToLower().Contains(specParams.Search))&&
               (!specParams.BrandId.HasValue || p.ProductBrandId== specParams.BrandId.Value)&&
                  (!specParams.TypeId.HasValue || p.ProductTypeId == specParams.TypeId.Value) 
               )
          
                    
        {

            //add the Inclouds of the Products
            Includs.Add(p => p.ProdectBrand);
            Includs.Add(p => p.ProductType);

            // add the sort way

            AddOrderBy(p => p.Name); // the Default sorting ig the user{frontEnd Or mobile} don't send sort value)

            if (specParams.Sort != null)
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                         AddOrderBy(p => p.Price);

                        // OrderBy = p=> p.Price;    // we can use the Property Direct if we don't Make A Mathod (OrderBy)
                        break;
                    case "priceDesc":
                        AddOrerByDesc(p => p.Price); 

                      //  OrderByDescending = p => p.Price;  // we can use the Property Direct if we don't Make A Mathod (OrderByDesc)
                        break;
                    default:
                        AddOrderBy(p=>p.Name); 
                        break;
                }

            }

            //add the Pagination way

            //Ex: If  pageSize = 10 / PageIndex = 3 => we will Skip 20 Product and Take The 10 after that
                              //<............skip 20......................>   //  <....take 10....>
            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);



        }


        //this constractor will use if we have Caritria (where) => for Get one Product EndPoint
        public ProductWithBrandAndTypeSpecification(int id):base(p=> p.Id == id) 
        {
            Includs.Add(p => p.ProdectBrand);
            Includs.Add(p => p.ProductType);
        }
    }
}
