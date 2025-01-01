using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
public class AccountController : Controller
{


	public IActionResult Index()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Logout()
	{
		var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
		ViewBag.IsLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
		ViewBag.IsLoginRole = HttpContext.Session.GetInt32("IdRole") != null;
		// Xóa dữ liệu session
		HttpContext.Session.Remove("IdMember");
		HttpContext.Session.Remove("IdRole");

		// Chuyển hướng đến trang đăng nhập hoặc trang chính
		return RedirectToAction("", "");
	}
}