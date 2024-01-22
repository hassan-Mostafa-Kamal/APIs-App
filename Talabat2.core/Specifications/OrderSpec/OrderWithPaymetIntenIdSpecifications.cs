using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.Specifications;
using Talabat2.core.Entities.Order_Aggregate;

namespace Talabat2.core.Specifications.OrderSpec
{
    public class OrderWithPaymetIntenIdSpecifications : BaseSpecification<Order>
    {
        public OrderWithPaymetIntenIdSpecifications(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }

    }
}
