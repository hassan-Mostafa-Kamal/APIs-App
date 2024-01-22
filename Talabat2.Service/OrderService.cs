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

namespace Talabat2.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepository,
                            //IGenericRepository<Product> productRepo,
                            //IGenericRepository<DeliveryMethod> DeliveryMethodRepo,
                            //IGenericRepository<Order>  OrderRepo
                            IUnitOfWork unitOfWork,
                            IPaymentService paymentService
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = DeliveryMethodRepo;
            //_orderRepo = OrderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1: Get Basket From BasketRepo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //2: Get selected Items at Basket From ProductRepo To Validete(price&

            var orderItems = new List<OrderItem>();

                   // basket?.Items?.Count > 0;
            if (basket != null && basket.Items != null && basket.Items.Count > 0)
            {

                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id); 

                    var productOrderItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productOrderItem, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                        
                }
            }


            //3: Calculate subTotal

            var subTotal = orderItems.Sum(item=> item.Price * item.Quantity);


            //4: Get deliveryMethod From DeliveryMethodRepo

            var deliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //5: chick if there was Existing order at the same basket with the same PaymentIntentId
            var spec = new OrderWithPaymetIntenIdSpecifications(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if(existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);

                  // to Update the PaymentIntent with new Amount
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }        
              

            //6: Create Order 

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);

            await _unitOfWork.Repository<Order>().Add(order);


            //7: Save Order at Data Base
           var result =  await  _unitOfWork.Complete();
            if (result <= 0) { return null; }

            return order;
        }



        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);

            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return Orders;
            }


        public async Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail,orderId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await  _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

               return deliveryMethods;
        }
    }
}
