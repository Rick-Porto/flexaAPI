using Microsoft.OpenApi.Models;
using System.Reflection;

namespace flexa.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                // Basic Config
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ConnectWeek API",
                    Version = "v1",
                    Description = "API Documentation - ConnectWeek"
                });

                // XML documentation (if using /// comments)
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);

                // JWT Bearer Authentication
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Insert 'Bearer {token}'",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            return services;
        }


        public static IApplicationBuilder UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "FLEXA API Docs";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "FlexaSwagger"; // http://localhost:5000/flexaSwagger

                // UI Optional Settings:
                options.DefaultModelsExpandDepth(-1); // Hide schemas
                options.DisplayRequestDuration();
            });

            return app;
        }

        }
    }
