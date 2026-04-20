using flexa.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Swagger via Exstension folder
builder.Services.AddSwaggerExtension();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.ConfigureDatabase(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();