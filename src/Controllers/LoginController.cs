using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services;
using Staffinfo.Divers.Services.Contracts;
using System.Threading.Tasks;

namespace staffinfo.divers.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager _userManager;

        public LoginController(IAccountService accountService, UserManager userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SignIn(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return View("Index", loginModel);

            var identity = await _accountService.LoginAsync(loginModel);

            if(identity == null)
            {
                ViewData["LoginError"] = "Неверный логин или пароль";
                return View("Index", loginModel);
            }

            return Redirect("/Dashboard");
        }

        public async Task<IActionResult> SignOut()
        {
            // set identity to local storage
            _userManager.ClearUserData();

            return RedirectToAction("Index", "Login");
        }
    }
}