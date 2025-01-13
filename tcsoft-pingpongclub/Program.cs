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
builder.Services.AddMemoryCache();
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
// Thêm Middleware biên dịch SCSS sang CSS
app.Use(async (context, next) =>
{
    // Kiểm tra nếu yêu cầu là file CSS
    if (context.Request.Path.Value.EndsWith(".css"))
    {
        string scssPath = Path.Combine("wwwroot", Path.ChangeExtension(context.Request.Path.Value, ".scss"));
       
        // Kiểm tra xem file SCSS có tồn tại không
        if (File.Exists(scssPath))
        {
            try
            {
                // Đọc nội dung file SCSS
                string scssContent = await File.ReadAllTextAsync(scssPath);

                // Biên dịch SCSS thành CSS
                var result = SassCompiler.Compile(scssContent);

                // Trả về CSS cho client
                context.Response.ContentType = "text/css";
                await context.Response.WriteAsync(result.CompiledContent);
                return;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có khi biên dịch SCSS
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Error compiling SCSS: {ex.Message}");
                return;
            }
        }
    }

    // Nếu không phải file CSS, tiếp tục với các Middleware khác
    await next();
});

// Cấu hình các Middleware mặc định
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
