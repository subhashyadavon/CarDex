using CarDexBackend.Services;


var builder = WebApplication.CreateBuilder(args);

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
    // use the MOCK service
    builder.Services.AddSingleton<IAuthService, MockAuthService>();
    builder.Services.AddSingleton<ICardService, MockCardService>();
    builder.Services.AddSingleton<ICollectionService, MockCollectionService>();
    builder.Services.AddSingleton<IPackService, MockPackService>();
    builder.Services.AddSingleton<ITradeService, MockTradeService>();
    builder.Services.AddSingleton<IUserService, MockUserService>();
}
else
{
    // use real PRODUCTION services
    //builder.Services.AddScoped<IAuthService, AuthService>();
    //builder.Services.AddScoped<ICardService, CardService>();
    //builder.Services.AddScoped<ICollectionService, CollectionService>();
    //builder.Services.AddScoped<IPackService, PackService>();
    //builder.Services.AddSingleton<ITradeService, TradeService>();
    //builder.Services.AddSingleton<IUserService, UserService>();
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