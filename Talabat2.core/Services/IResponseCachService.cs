using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat2.core.Services
{
    public interface IResponseCachService
    {
        Task CashResponseAsync(string cashKey, object response, TimeSpan timeToLive);

        Task<string> GetCashedResponseAsync(string cashKey);

    }
}
