using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Service;
using tcsoft_pingpongclub.Filter;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<ThuctapKtktcn2024Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectedDb")));

// Thêm dịch vụ vào DI container
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

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kích hoạt middleware Session
app.UseSession();

// Kích hoạt Authorization
app.UseAuthorization();

// Định tuyến controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "permissionRole",
    pattern: "{controller=PermissionRoles}/{action=Index}/{id?}");

app.Run();
