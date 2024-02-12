using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetAll(string role)
        {
            IEnumerable<ApplicationUser> list;
            ResponseDto response = _userService.GetAllAsync().GetAwaiter().GetResult();
            string userId = "";
            if (response.Result != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ApplicationUser>>(Convert.ToString(response.Result));
                switch (role)
                {
                    case "admin":
                        list = list.Where(u => u.Role == SD.RoleAdmin);
                        break;
                    case "customer":
                        list = list.Where(u => u.Role == SD.RoleCustomer);
                        break;
                    default: 
                        break;
                }
            }
            else
            {
                list = new List<ApplicationUser>();
            }
            return Json(new { data = list });
        }
    }
}
