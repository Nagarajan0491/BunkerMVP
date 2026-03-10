using BunkerMVP.API.Middleware;
using BunkerMVP.API.Services;
using BunkerMVP.Infrastructure.Data;
using BunkerMVP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// DbContext
builder.Services.AddDbContext<BunkerDbContext>(opt => opt.UseNpgsql(connStr));

// Application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VesselService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<BunkerRequestService>();
builder.Services.AddScoped<DashboardService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200","http://localhost:52716")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;  
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;  // Set to None for local dev (use Always in production with HTTPS)
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".BunkerMVP.Session";  // Explicit cookie name
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BunkerMVP API", Version = "v1" });
});
builder.Services.AddHttpClient();
builder.Services.AddHostedService<ChatbotActionRegistrar>();

var app = builder.Build();

// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BunkerDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BunkerMVP API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAngular");
app.UseSession();
app.UseMiddleware<AuthMiddleware>();
app.MapControllers();

app.Run();
