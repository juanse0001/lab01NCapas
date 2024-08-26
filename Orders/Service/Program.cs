using BLL;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<Customers>();
builder.Services.AddTransient<Suppliers>();
builder.Services.AddTransient<Products>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore-swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configuración de la localización (cultura)
var supportedCultures = new[] { new CultureInfo("es-ES") }; // Cultura española
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-ES"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseAuthorization();

app.MapControllers();

// Debe ser llamado antes de UseEndpoints
app.UseRouting(); // Middleware de autorización

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "api/{controller}/{action}/{id?}");
});

app.Run();
