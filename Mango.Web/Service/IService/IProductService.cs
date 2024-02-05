using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetAllCouponsAsync(); 
        Task<ResponseDto?> GetCouponByIdAsync(int id); 
        Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto); 
        Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto); 
        Task<ResponseDto?> DeleteCouponsAsync(int id); 
    }
}
