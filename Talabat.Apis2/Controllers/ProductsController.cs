using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis2.Dtos;
using Talabat.Apis2.ErrorsResponseHandling;
using Talabat.Apis2.Helpers;
using Talabat2.core;
using Talabat2.core.Entities;
using Talabat2.core.Repositories;
using Talabat2.core.Specifications;

namespace Talabat.Apis2.Controllers
{

   
    public class ProductsController : ApiControllerBase
    {

        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        //private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ProductsController(
            //IGenericRepository<Product> productsRepo
            //,IGenericRepository<ProductBrand> productBrandRepo
            //,IGenericRepository<ProductType> productTypeRepo
            IUnitOfWork unitOfWork
            ,IMapper mapper)
        {
            //_productsRepo = productsRepo;
            //_productBrandRepo = productBrandRepo;
            //_productTypeRepo = productTypeRepo;
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }
        //[Authorize]
        [CashedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            //Firest Request To Data base
            var spec = new ProductWithBrandAndTypeSpecification(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            //secand Request To Data base To Get Count 
            var countSpec = new ProductWithFilterationForCountSpecification(specParams);
            int count =await _unitOfWork.Repository<Product>().GetCountWithSpacAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(data,specParams.PageIndex,specParams.PageSize,count));
            
        }



        #region Improve Swagger Decomantation

       
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK /*200*/)]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status404NotFound /*404*/)]
        #endregion
        [CashedAttribute(600)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);


            #region //chack Exception Handling
            //var product2 = await _productsRepo.GetByIdAsync(100);
            //var productToReturn =  product2.ToString();
            #endregion


            if (product == null)
            {
                return BadRequest(new ApiErrorResponse(400));
            }
             return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
          //  return Ok(product);
        }


        [CashedAttribute(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {


            var brands =await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(brands);
        }


        [CashedAttribute(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types =await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
         
    }
}
