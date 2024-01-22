using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat2.core.Services;

namespace Talabat.Apis2.Helpers
{
    public class CashedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSecend;

        public CashedAttribute(int timeToLiveInSecend)
        {
            _timeToLiveInSecend = timeToLiveInSecend;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cashServiceObject = context.HttpContext.RequestServices.GetRequiredService<IResponseCachService>();

            var cashKey = GenerateCashKeyFromRequest(context.HttpContext.Request);
            var cashedResponse = await cashServiceObject.GetCashedResponseAsync(cashKey);

            if (!string.IsNullOrEmpty(cashedResponse))
            {

                var contentResult = new ContentResult()
                {
                    Content = cashedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            else
            {
                var executedEndpointContext = await next.Invoke();
                if (executedEndpointContext.Result is OkObjectResult okObjectResult)
                {
                    await cashServiceObject.CashResponseAsync(cashKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSecend));

                }

            }



        }

        private string GenerateCashKeyFromRequest(HttpRequest request)
        {
            //                   1: urlPath   //2: QueryString 
            //https://localhost:7261/api/products?pageIndex=1&pageSize=5&sort=name

            var KeyBuilder = new StringBuilder();

            KeyBuilder.Append(request.Path);  ///api/products

            //pageIndex=1
            //&pageSize=5
            //&sort=name
            foreach (var (key, value) in request.Query.OrderBy(k=> k.Key))
            {
                KeyBuilder.Append($"| {key} - {value}");
                ///api/products |pageIndex-1
                ///api/products |pageIndex-1 |pageSize-5
                ///api/products |pageIndex-1 | pageIndex-1 |pageSize-5 | sort-name

            }
            return KeyBuilder.ToString();


        }
    }
}
