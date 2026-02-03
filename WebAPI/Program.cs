using CoreLogic.Extensions;      // ← Расширения доменного слоя
using DataAccess;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi;
using WebAPI.Configuration;
using WebAPI.Extensions;         // ← Расширения инфраструктурного слоя

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

string authConnectionString = builder.Configuration.GetConnectionString("AuthConnection")
    ?? throw new InvalidOperationException("Auth db connection string not configured");

string stockConnectionString = builder.Configuration.GetConnectionString("StocktakingConnection")
    ?? throw new InvalidOperationException("Stocktaking db connection string not configured");

// Регистрация инфраструктурных компонентов
builder.Services.AddDataAccess(authConnectionString);
builder.Services.AddStockDataAccess(stockConnectionString);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
// Регистрация инфраструктурных сервисов
builder.Services.AddTokenService();
builder.Services.AddJwtAuthentication();
builder.Services.AddAuthorization(options => options.AddCustomPolicies());

// Регистрация доменных сервисов
builder.Services.AddCoreLogicServices();
builder.Services.AddInventoryServices();

// Другие сервисы
builder.Services.AddHttpContextAccessor(); // Для получения имени пользователя в MoveDevice

// CORS, контроллеры, Swagger
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "AssetKeeper API", 
        Version = "v1" 
    });
    
    // SecurityScheme - ИСПРАВЛЕНО ДЛЯ SWASHBUCKLE 10.x
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AssetKeeper API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
    });
}

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();