using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;
        private ResponseDto _response;
        public CartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }
        [HttpPost("Cartupset")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().
                    FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    if(cartHeaderFromDb == null)
                    {
                        // create header and details
                        CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                        _db.CartHeaders.Add(cartHeader);
                        await _db.SaveChangesAsync();
                        cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // check if datails has same product
                        var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                            u => u.ProductId == cartDto.CartDetails.First().ProductId && 
                            u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                        if(cartDetailsFromDb == null)
                        {
                            // create cartdetails
                            cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                            _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                            await _db.SaveChangesAsync();
                        }
                        else
                        {
                            // update count in cart details
                            cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                            cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                            cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                            _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                            await _db.SaveChangesAsync();
                        }
                    }
                    _response.Result = cartDto;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
