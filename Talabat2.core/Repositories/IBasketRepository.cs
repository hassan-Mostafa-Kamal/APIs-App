using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities;

namespace Talabat2.core.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);

        Task<bool> DeleteBasketAsync(string basketId);


        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
    }
}
