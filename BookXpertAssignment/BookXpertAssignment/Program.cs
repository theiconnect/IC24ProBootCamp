using BookXpertAssignment.DAL;
using BookXpertAssignment.Models.ModelsUsingEFCore;
using BookXpertAssignment.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<BookXpertAssignmentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookXpertConnectionString")));

builder.Services.AddTransient<EmployeeRepository>();
builder.Services.AddTransient<EmployeeServices>();

// Add services to the container.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
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
	pattern: "{controller=Employee}/{action=EmployeeList}/{id?}");

app.Run();
