using RMSNextGen.Services;
using RMSNextGen.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

string connectionstring = builder.Configuration.GetConnectionString("RMSNextGenDB");
builder.Services.AddTransient<StoreRepository>(provider =>
	new StoreRepository(connectionstring));

builder.Services.AddTransient<StoreService>();



// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
string connectionString = builder.Configuration.GetConnectionString("RMSNextGenConnectionString");

builder.Services.AddTransient<EmployeeRepository>(provider => new EmployeeRepository(connectionString));
builder.Services.AddTransient<EmployeeService>();

builder.Services.AddTransient<ProductServices>();

//Product Category Module
builder.Services.AddTransient<ProductCategoryRepository>(provider => new ProductCategoryRepository(connectionString));

builder.Services.AddTransient<ProductCategoryServices>();

//Stock Module


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
