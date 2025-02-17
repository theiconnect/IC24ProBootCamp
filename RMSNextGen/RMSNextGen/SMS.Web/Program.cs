using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SMS.DAL;
using SMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
<<<<<<< HEAD
string ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient<UserRepository>(provider =>
    new UserRepository(builder.Configuration.GetConnectionString("SMSDBConnectionString")));


builder.Services.AddTransient<GVijayStudentRepository>(provider =>
	new GVijayStudentRepository(builder.Configuration.GetConnectionString("SMSDBConnectionString")));

builder.Services.AddTransient<GVijayStudentService>();

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<LokeshStudentRepository>(provider =>
    new LokeshStudentRepository(connectionString));

builder.Services.AddTransient<LokeshStudentService>();

builder.Services.AddTransient<SaiStudentRepository>(provider =>
	new SaiStudentRepository(connectionString));
builder.Services.AddTransient<SaiStudentService>();
>>>>>>> main

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
    });

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
