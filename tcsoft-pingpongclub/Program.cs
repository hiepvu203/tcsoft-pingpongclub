using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Service;
using tcsoft_pingpongclub.Filter;
using LibSassHost; // Thêm thư viện LibSassHost vào
using tcsoft_pingpongclub.Hubs; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
// Cấu hình DbContext
// Cấu hình mã hóa UTF-8
Console.OutputEncoding = System.Text.Encoding.UTF8;

builder.Services.AddDbContext<ThuctapKtktcn2024Context>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectedDb")));

builder.Services.AddControllersWithViews();

// Cấu hình Session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
	options.Cookie.HttpOnly = true; // Tăng cường bảo mật
	options.Cookie.IsEssential = true; // Đảm bảo cookie hoạt động
});

// Đăng ký AuthorizationService và AuthorizationFilter vào DI container
builder.Services.AddScoped<IsAuthorized, AuthorizationService>();
builder.Services.AddScoped<AuthorizationFilter>();
builder.Services.AddScoped<MenuActionFilter>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Thêm middleware session
app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "permissionRole",
	pattern: "{controller=PermissionRoles}/{action=Index}/{id?}");
app.MapHub<SetRatio>("/setRatio");
app.Run();
