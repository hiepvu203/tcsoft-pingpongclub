using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Filter;
using tcsoft_pingpongclub.Service;

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
		
		public async Task<IActionResult> HienThiThongTinGiaiDau()
		{
			var tournamentsWithActualAmount = await _context.Tournaments
				.Include(t => t.RankEndNavigation)
				.Include(t => t.RankStartNavigation)
				.Select(t => new Tournament
				{
					IdTournament = t.IdTournament,
					TournamentName = t.TournamentName,
					UrlImage = t.UrlImage,
					TimeStart = t.TimeStart,
					TimeEnd = t.TimeEnd,
					Infor=t.Infor,
					Amount=t.Amount,
					RankStartNavigation = t.RankStartNavigation,
					RankEndNavigation = t.RankEndNavigation,
					Status = t.Status,
					// Tính ActualAmount bằng cách đếm số Players liên quan
					ActualAmount = (short)_context.Players.Count(p => p.IdTournament == t.IdTournament)
				})
				.OrderByDescending(t => t.IdTournament)
				.ToListAsync();

			return View(tournamentsWithActualAmount);
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
