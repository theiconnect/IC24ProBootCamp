using RMSNextGen.DAL;
using RMSNextGen.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();
string connectionString = builder.Configuration.GetConnectionString("RMSNextGenConnectionString");



//builder.Services.AddTransient<UserRepository>(provider =>
//	new UserRepository(connectionString));

////builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<EmployeeRepository>(provider =>
	new EmployeeRepository(connectionString));
builder.Services.AddTransient<EmployeeService>();


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
