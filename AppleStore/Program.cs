//using AppleStore.DataAcess;
//using AppleStore.Models;
//using AppleStore.Repository;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddDistributedMemoryCache();


//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//.AddDefaultTokenProviders()
//.AddDefaultUI()
//.AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddRazorPages();

//builder.Services.AddSession(options =>
//{
//	options.IdleTimeout = TimeSpan.FromMinutes(15);
//	options.Cookie.HttpOnly = true;
//	options.Cookie.IsEssential = true;
//});
//builder.Services.AddHttpContextAccessor();


//// Add services to the container.
//builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<IProductRepository, EFProductRepository>();
//builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();


//var app = builder.Build();
//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//app.UseStaticFiles();


//// Đặt trước UseRouting
//app.UseSession();


//app.UseRouting();


//app.UseAuthentication(); 


//app.UseAuthorization();

//app.MapRazorPages();


//app.UseEndpoints(endpoints =>
//{
//	_ = endpoints.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}");
//});


//app.Run();


using AppleStore.DataAcess;
using AppleStore.Models;
using AppleStore.Repository;
using AppleStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSingleton<IVnPayService, VnPayService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddDefaultTokenProviders()
 .AddDefaultUI()
 .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
	// Đọc thông tin Authentication:Google từ appsettings.json
	//IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

	// Thiết lập ClientID và ClientSecret để truy cập API google
	googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
	googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
	// Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
	//googleOptions.CallbackPath = "/signin-google";

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseAuthorization();

app.UseSession();
//app.UseEndpoints(endpoints =>
//{
//    _ = endpoints.MapControllerRoute(
//        name: "Admin",
//        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
//    );

//    _ = endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});
app.MapControllerRoute(
	name: "admin",
	pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
// Các middleware khác...

app.MapRazorPages();

app.Run();