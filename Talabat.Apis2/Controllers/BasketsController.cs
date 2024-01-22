using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis2.Dtos;
using Talabat.Apis2.ErrorsResponseHandling;
using Talabat2.core.Entities;
using Talabat2.core.Repositories;

namespace Talabat.Apis2.Controllers
{
   
    public class BasketsController : ApiControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}") ]                              //GetOrRecreate if it expired
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            

            var basket =  await _basketRepository.GetBasketAsync(id);
            return basket is null? new CustomerBasket(id) : basket;

        }

        [HttpPost]                                     //create or Update basket if it already Exsist
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket =  _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);

            var createdOrUpdateBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);

            if (createdOrUpdateBasket == null) { return BadRequest(new ApiErrorResponse(400)); }
            return Ok(createdOrUpdateBasket);

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>>  DeleteBasket(string id)
        {

            return await _basketRepository.DeleteBasketAsync(id);
        }




    }
}
