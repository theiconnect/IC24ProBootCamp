using RMSNextGen.Services;
using RMSNextGen.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Caching.Memory;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
//Add in memory Cache
builder.Services.AddMemoryCache();
// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("RMSNextGenConnectionString");
//Lookup
builder.Services.AddTransient<LookupRepository>(provider => new LookupRepository(connectionString));
builder.Services.AddTransient<LookupService>();
//Store
builder.Services.AddTransient<StoreRepository>(provider => new StoreRepository(connectionString));
builder.Services.AddTransient<StoreService>();
//Employee
builder.Services.AddTransient<EmployeeRepository>(provider => new EmployeeRepository(connectionString));
builder.Services.AddTransient<EmployeeService>();
//Product
builder.Services.AddTransient<ProductRepository>(provider =>
{
	var memoryCache = provider.GetRequiredService<IMemoryCache>();
	return new ProductRepository(connectionString, memoryCache);
});
//builder.Services.AddTransient<ProductRepository>(provider => new ProductRepository(connectionString, IMemoryCache));
builder.Services.AddTransient<ProductServices>();
//Product Category
builder.Services.AddTransient<ProductCategoryRepository>(provider => new ProductCategoryRepository(connectionString));
builder.Services.AddTransient<ProductCategoryServices>();

builder.Services.AddTransient<LoginRepository>(provider => new LoginRepository(connectionString));
builder.Services.AddTransient<LoginServices>();


//AddAuthentication and Add Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{

		options.LoginPath = "/Login/Login"; // Redirect to this if not logged in
		//options.LogoutPath = "/Login/Logout";
		options.AccessDeniedPath = "/Login/AccessDenied";
		//options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
		//The login session (cookie) will automatically expire after 30 minutes of inactivity.



	});
builder.Services.AddAuthorization();

//JWT Authentication
//var jwtKey = builder.Configuration["Jwt:Key"];
//var jwtIssuer = builder.Configuration["Jwt:Issuer"];
//var jwtAudience = builder.Configuration["Jwt:Audience"];

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//	.AddJwtBearer(options =>
//	{
//		options.RequireHttpsMetadata = false;
//		options.SaveToken = true;
//		options.TokenValidationParameters = new TokenValidationParameters
//		{
//			ValidateIssuer = true,
//			ValidateAudience = true,
//			ValidateLifetime = true,
//			ValidateIssuerSigningKey = true,
//			ValidIssuer = jwtIssuer,
//			ValidAudience = jwtAudience,
//			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//		};
//	});
//builder.Services.AddSession();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();





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

app.UseSession();


// ?? Add auth middleware
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
