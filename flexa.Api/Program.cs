using flexa.Api.Extensions;
using flexa.Api.Middleware;
using flexa.Utils.Converters;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT");
    if (port is not null)
    {
        options.ListenAnyIP(int.Parse(port));
    }
    else
    {
        options.ListenAnyIP(5000);
        options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps());
    }
});

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        var origins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddSwaggerExtension();
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new BrazilDateTimeConverter());
    });

// Register AutoMapper before application services that depend on IMapper
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApplicationServices();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddSupabaseAuth(builder.Configuration);
builder.Services.AddAuthorization();
// Ensure the existing Bearer scheme uses the token's 'sub' claim as the name identifier
builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.TokenValidationParameters ??= new TokenValidationParameters();
        options.TokenValidationParameters.NameClaimType = "sub";
        options.TokenValidationParameters.RoleClaimType = "role";
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{   
    app.UseSwaggerExtension();
    IdentityModelEventSource.ShowPII = true;
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();