using CoreLogic;
using CoreLogic.Models.Configuration;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("AuthConnection")
    ?? throw new InvalidOperationException("Auth db connection string not configured");
builder.Services.AddDataAccess(connectionString);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings not configured");
builder.Services.AddTokenService(jwtSettings);
builder.Services.AddAuth(jwtSettings);

builder.Services.AddControllers();
builder.Services.AddCoreLogic();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();