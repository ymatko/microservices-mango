using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult UserIndex()
        {
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetAll(string role)
        {
            IEnumerable<ApplicationUserDto> list;
            ResponseDto response = _userService.GetAllAsync().GetAwaiter().GetResult();
            string userId = "";
            if (response.Result != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ApplicationUserDto>>(Convert.ToString(response.Result));
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
                list = new List<ApplicationUserDto>();
            }
            return Json(new { data = list });
        }
        [HttpGet]
        public async Task<IActionResult> UserDetail(string userId)
        {
            ApplicationUserDto user = new ApplicationUserDto();
            var response = await _userService.GetUserAsync(userId);
            if (response.IsSuccess)
            {
                user = JsonConvert.DeserializeObject<ApplicationUserDto>(Convert.ToString(response.Result));
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(ApplicationUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _userService.Update(userDto);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product user successfully updated";
                    return RedirectToAction(nameof(UserIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(userDto);
        }
    }
}
