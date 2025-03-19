using RMSNextGen.Services;
using RMSNextGen.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

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
builder.Services.AddTransient<ProductRepository>(provider => new ProductRepository(connectionString));
builder.Services.AddTransient<ProductServices>();
//Product Category
builder.Services.AddTransient<ProductCategoryRepository>(provider => new ProductCategoryRepository(connectionString));
builder.Services.AddTransient<ProductCategoryServices>();



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
