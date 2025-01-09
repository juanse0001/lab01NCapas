using Microsoft.AspNetCore.Localization;
using ProxyService.Interfaces;
using ProxyService;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registro de servicios y proxies
builder.Services.AddScoped<ICustomerProxy, CustomerProxy>();
builder.Services.AddScoped<IProductProxy, ProductProxy>();
builder.Services.AddScoped<ISupplierProxy, SupplierProxy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configuración de la localización (cultura)
var supportedCultures = new[] { new CultureInfo("es-ES") }; // Puedes ajustar esto a la cultura que prefieras
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-ES"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
