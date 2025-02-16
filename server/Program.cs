using Context;
using Factories;
using Mappers;
using Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add configuration sources
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var provider = builder.Configuration["Provider"];
switch (provider)
{
    case "Sqlite":
        {
            builder.Services.AddDbContext<GameContext, SqliteGameContext>();
            break;
        }
    case "SqlServer":
        {
            builder.Services.AddDbContext<GameContext, SqlServerGameContext>();
            break;
        }
    default:
        {
            throw new ArgumentException($"Invalid provider: {provider}");
        }
}

builder.Services.AddScoped<EntityMapper>();
builder.Services.AddScoped<ModelMapper>();
builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<WorldRepository>();
builder.Services.AddScoped<MapFactory>();
builder.Services.AddScoped<DefaultWorldFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add JWT authentication
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException("Jwt:Key", "JWT Key is not configured.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
    ValidateIssuer = false,
    ValidateAudience = false
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
}

app.UseAuthentication(); // Add this line to enable authentication middleware
app.UseAuthorization();
app.MapControllers();
app.Run();