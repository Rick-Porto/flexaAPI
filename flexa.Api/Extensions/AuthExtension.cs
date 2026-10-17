using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace flexa.Api.Extensions;

public static class AuthExtension
{
    public static IServiceCollection AddSupabaseAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSecret = configuration["Supabase:JwtSecret"]
            ?? throw new InvalidOperationException("Supabase:JwtSecret is missing.");

        var supabaseUrl = configuration["Supabase:Url"]
            ?? throw new InvalidOperationException("Supabase:Url is missing.");

        // Raw UTF-8 bytes — this is confirmed correct by manual signature verification.
        var hs256Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenHandlers.Clear();
            options.TokenHandlers.Add(new JwtSecurityTokenHandler()); // legacy, handles / in kid correctly

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = hs256Key,

                ValidateIssuer = false,
                ValidIssuer = $"{supabaseUrl.TrimEnd('/')}/auth/v1",

                ValidateAudience = false,
                ValidAudience = "authenticated",

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            // Debug events — logs the EXACT failure reason to the console/output window.
            // Remove these once auth is working.
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = ctx =>
                {
                    Console.WriteLine($"[JWT FAILED] {ctx.Exception.GetType().Name}: {ctx.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = ctx =>
                {
                    Console.WriteLine($"[JWT OK] sub={ctx.Principal?.FindFirst("sub")?.Value}");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}