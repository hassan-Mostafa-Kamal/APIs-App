using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.FinancialConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core;
using Talabat2.core.Entities;
using Talabat2.core.Entities.Order_Aggregate;
using Talabat2.core.Repositories;
using Talabat2.core.Services;
using Talabat2.core.Specifications.OrderSpec;
using Product = Talabat2.core.Entities.Product;

namespace Talabat2.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork
            )
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {

            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];


            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;


            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                basket.ShippingCost = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }


            }




            //refrance of class from Stripe Backage
            PaymentIntent paymentIntent;
            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))  //if true we are Create PaymentIntent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)basket.ShippingCost * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }

                };
                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else  //else we are Update PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)basket.ShippingCost * 100

                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }


            await _basketRepository.UpdateBasketAsync(basket);

            return basket;


        }

        // this Fun for WebHook EndPoint
        public async Task<Order> UpdatePaymentIntentToSuccededOrFailed(string paymentIntentId, bool isSucceeded)
        {

            var spec = new OrderWithPaymetIntenIdSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);


            if (isSucceeded)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
             _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();

            return order;
        }
    }
}
