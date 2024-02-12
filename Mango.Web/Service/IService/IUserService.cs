using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IUserService
    {
        Task<ResponseDto?> GetAllAsync();

    }
}
