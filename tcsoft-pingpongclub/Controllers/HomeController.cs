using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Filter;
namespace tcsoft_pingpongclub.Controllers
{
	[ServiceFilter(typeof(MenuActionFilter))]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ThuctapKtktcn2024Context _context;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
			_context=new ThuctapKtktcn2024Context();
		}

		public IActionResult Index()
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			ViewBag.IsLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			
			return View();
		}
	  


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
