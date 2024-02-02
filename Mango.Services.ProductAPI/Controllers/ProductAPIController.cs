using AutoMapper;
using Mango.Services.ProductApi.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        private readonly ILogger<ProductAPIController> _logger;

        public ProductAPIController(AppDbContext db, IMapper mapper, ILogger<ProductAPIController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _response = new ResponseDto();
        }
        [HttpGet]
        [Tags("Getters")]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Product.Get " + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("{id:int}")]
        [Tags("Getters")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product obj = _db.Products.First(o => o.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Product.GetById " + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost]
        [Tags("Creators")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post(ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _db.Products.Add(product);
                _db.SaveChanges();

                if(productDto.Image != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + filePath;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _db.Products.Update(product);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Product.Post " + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPut]
        [Tags("Updaters")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto product)
        {
            try
            {
                Product obj = _mapper.Map<Product>(product);
                _db.Products.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Product.Put " + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpDelete ]
        [Route("{id:int}")]
        [Tags("Deleters")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(o => o.ProductId == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Product.Delete " + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
