using CoreLogic.Extensions;      // ← Расширения доменного слоя
using DataAccess;
using WebAPI.Configuration;
using WebAPI.Extensions;         // ← Расширения инфраструктурного слояusing DataAccess;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();