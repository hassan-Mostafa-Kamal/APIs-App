using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Apis2.Dtos;
using Talabat.Apis2.ErrorsResponseHandling;
using Talabat2.core.Entities.Order_Aggregate;
using Talabat2.core.Services;

namespace Talabat.Apis2.Controllers
{
    public class PaymentsController : ApiControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _wHSecret = "whsec_f625337e8c83b35737c334cbc8df68e4393f1495ba843bc084a1c3b8929883d6";


        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]

        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK /*200*/)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest /*404*/)]
        [HttpPost("{basketId}")]

        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {

            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) { return BadRequest(new ApiErrorResponse(400, "A problem with your Basket")); }

            return Ok(basket);
        }


        [HttpPost("webhook")]

        public async Task<IActionResult> StripWebhook()
        {


            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _wHSecret);
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            Order order;
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order = await _paymentService.UpdatePaymentIntentToSuccededOrFailed(paymentIntent.Id, true);
                    _logger.LogInformation("Payment Is Succeeded Ya Hamde", paymentIntent.Id);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdatePaymentIntentToSuccededOrFailed(paymentIntent.Id, false);
                    _logger.LogInformation("Payment Is Failed Ya Hamde :(", paymentIntent.Id);


                    break;
            }

            return Ok();

        }
    }
}
