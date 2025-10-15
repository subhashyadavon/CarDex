using CarDexBackend.Services;
using CarDexDatabase;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Database Context with PostgreSQL
builder.Services.AddDbContext<CarDexDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CarDexDatabase"))
);

// add services to the container
builder.Services.AddControllers();

// register Swagger to test endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add CORS for our frontend(s)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


// Environment switching for development and production
if (builder.Environment.IsDevelopment())
{
    // Use PRODUCTION services with real database (Supabase PostgreSQL)
    builder.Services.AddSingleton<IAuthService, MockAuthService>(); // Keep mock for now (no auth implemented yet)
    builder.Services.AddScoped<ICardService, CardService>();
    builder.Services.AddScoped<ICollectionService, CollectionService>();
    builder.Services.AddScoped<IPackService, PackService>();
    builder.Services.AddScoped<ITradeService, TradeService>();
    builder.Services.AddScoped<IUserService, UserService>();
}
else
{
    // use real PRODUCTION services
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ICardService, CardService>();
    builder.Services.AddScoped<ICollectionService, CollectionService>();
    builder.Services.AddScoped<IPackService, PackService>();
    builder.Services.AddScoped<ITradeService, TradeService>();
    builder.Services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

// Enable Swagger UI in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();