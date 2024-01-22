using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.Specifications;
using Talabat2.core.Entities.Order_Aggregate;

namespace Talabat2.core.Specifications.OrderSpec
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        public OrderSpecifications(string email )
            : base (o => o.BuyerEmail == email )
        {

            Includs.Add(o => o.DeliveryMethod);
            Includs.Add(o => o.Items);
            AddOrerByDesc(o => o.OrderDate);



        }

        public OrderSpecifications(string email , int id) : 
            base(o => o.BuyerEmail == email &&  o.Id == id)
        {
            Includs.Add(p => p.DeliveryMethod);
            Includs.Add(p => p.Items);
        }
    }
}
