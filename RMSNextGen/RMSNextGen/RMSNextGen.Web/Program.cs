using RMSNextGen.DAL;
using RMSNextGen.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
String ConnectionString = builder.Configuration.GetConnectionString("RMSNextGenDBConnection");
builder.Services.AddTransient<ProductCategoryRepository>(provider => new ProductCategoryRepository(ConnectionString));
builder.Services.AddTransient<ProductCategoryServices>();
builder.Services.AddTransient<StockRepository>(provider => new StockRepository(ConnectionString));  
builder.Services.AddTransient<StockServices>();

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
