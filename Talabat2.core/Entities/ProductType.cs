using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat2.core.Entities
{
    public class ProductType:BaseEntity
    {
        public string  Name { get; set; }

        //public ICollection<Product> Products { get; set; }
                                                             //we dont Make That Becousse we dont need it in our business (dont need to access the Prodects throw Brand)
                                                             // so we will configer this relation by fluant Api 
                                                             // becouse if we dont do that the Ef will translet it to one to one 


    } 
}
