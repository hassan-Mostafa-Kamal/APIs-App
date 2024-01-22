using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities;
using Talabat2.core.Entities.Order_Aggregate;

namespace Talabat2.core.Services
{
    public interface IPaymentService
    {

        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntentToSuccededOrFailed(string paymentIntentId, bool isSucceeded);
    }
}
