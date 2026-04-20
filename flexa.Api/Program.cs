using flexa.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Swagger via Exstension folder
builder.Services.AddSwaggerExtension();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.ConfigureDatabase(builder.Configuration);

// AUTH Services
builder.Services.AddSupabaseAuth(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension();
}

app.UseHttpsRedirection();

//// If using ASP.NET Core's built-in authentication/authorization, these would be necessary.
app.UseAuthentication();   // If using ASP.NET Core auth schemes   
app.UseAuthorization();

app.MapControllers();
app.Run();