using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;

var builder = WebApplication.CreateBuilder(args);

// K?t n?i c? s? d? li?u
builder.Services.AddDbContext<ThuctapKtktcn2024Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectedDb")));

// Th�m d?ch v? cho session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian timeout c?a session
    options.Cookie.HttpOnly = true;                // T?ng b?o m?t b?ng c�ch ch? cho ph�p truy c?p HTTP
    options.Cookie.IsEssential = true;            // ??m b?o cookie c?a session lu�n ???c s? d?ng
});

// Th�m c�c d?ch v? MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// C?u h�nh pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// K�ch ho?t session
app.UseSession();

app.UseAuthorization();

// C?u h�nh route m?c ??nh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
