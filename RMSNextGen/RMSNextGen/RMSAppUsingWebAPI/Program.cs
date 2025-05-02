using RMSAppUsingWebAPI.DAL;
using Microsoft.AspNetCore.ResponseCaching;
using RMSAppUsingWebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure CORS
////builder.Services.AddCors(options =>
////{
////	options.AddPolicy("AllowAll", policy =>
////	{


////		policy.WithOrigins("http://localhost:62052", "http://localhost:5046", "https://localhost:7056")
////			.AllowAnyMethod()
////			.AllowAnyHeader();
////	});
////});
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		policy =>
		{
			policy.WithOrigins(
				"http://localhost:62052", // Your MVC project (IIS)
				"http://localhost:5046", // Your MVC project (Kestrel)
				"https://localhost:7056",
				"https://localhost:44320"// Your Web API (HTTPS)
			)
			.AllowAnyHeader()
			.AllowAnyMethod();
		});
});



string connectionString = builder.Configuration.GetConnectionString("RMSNextGenConnectionString");

// 1. Bind JwtSettings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>(); // Get settings outside


// 2. Add Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings.Issuer,
		ValidAudience = jwtSettings.Audience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

		RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"  // Correct claim type

	};
});
// Add authorization
builder.Services.AddAuthorization();
//Add Swagger
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "RMSAppUsingWebAPI", Version = "v1" });

	// Add JWT support in Swagger
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer' followed by space and your JWT token.\r\n\r\nExample: \"Bearer eyJhbGci...\""
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});




builder.Services.AddScoped<StoreApiRepository>(provider => new StoreApiRepository(connectionString));

builder.Services.AddScoped<LoginApiRepository>(provider => new LoginApiRepository(connectionString));

builder.Services.AddResponseCaching();  // Enable Response Caching












var app = builder.Build();

// Use CORS middleware
//app.UseCors("AllowAll");
app.UseCors("AllowAllOrigins");

app.UseResponseCaching(); // Applies the response caching middleware



// Swagger config
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
