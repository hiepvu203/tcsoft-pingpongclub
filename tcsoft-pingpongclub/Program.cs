using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using LibSassHost; // Thêm thư viện LibSassHost vào

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext
builder.Services.AddDbContext<ThuctapKtktcn2024Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectedDb")));

// Thêm dịch vụ cho MVC
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

// Cấu hình route mặc định cho MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
