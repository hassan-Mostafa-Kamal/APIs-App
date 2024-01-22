using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;
using Talabat.Apis2.Dtos;
using Talabat.Apis2.ErrorsResponseHandling;
using Talabat2.core;
using Talabat2.core.Entities.Order_Aggregate;
using Talabat2.core.Services;

namespace Talabat.Apis2.Controllers
{
    [Authorize]
    public class OrdersController : ApiControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
       // private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService,IMapper mapper/*IUnitOfWork unitOfWork*/)
        {
            _orderService = orderService;
            _mapper = mapper;
           // _unitOfWork = unitOfWork;
        }


        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        { 
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDto, Address>(orderDto.shippingAddress);
            var order =await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);
            if (order == null)
            {
                return BadRequest(new ApiErrorResponse(400));
            }

            return Ok(_mapper.Map<Order,OrderToReturnDto>( order));

        }


        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmil = User.FindFirstValue(ClaimTypes.Email);

            var Orders = await _orderService.GetOrdersForUserAsync(buyerEmil);

                
            return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>> (Orders));
        }

        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdForUserAsync(buyerEmail, id);

            if (order == null) { return NotFound(new ApiErrorResponse(404)); }

            return Ok(_mapper.Map<Order,OrderToReturnDto>( order));   
        }



        //[AllowAnonymous]
        //[HttpGet("deliveryMethods")]

        //public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        //{

        //    var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        //    return Ok(deliveryMethods);

        //}

        [HttpGet("deliveryMethods")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {

            var deliveryMethods =await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);

        }




    }
}
