using Microsoft.AspNetCore.Mvc;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class LoginController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;
        public LoginController()
        {
            _context = new ThuctapKtktcn2024Context();
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdMember") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var user = _context.Members.FirstOrDefault(m => m.Username == username && m.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("IdMember", user.IdMember);
                HttpContext.Session.SetInt32("IdRole", user.IdRole ?? 0);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
