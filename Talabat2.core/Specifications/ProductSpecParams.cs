using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat2.core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 10;

       

        public int? BrandId { get; set; }

        public int? TypeId { get; set; }



        private string?  search;

        public string?  Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }




        public string? Sort { get; set; }


        public int PageIndex { get; set; } = 1; //Dafult Page Index If The Front End Don't set it(I will Get First Page)

        private int pageSize = 5; //Dafult Page Size If The Front End Don't set it

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
      

    }
}
