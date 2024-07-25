using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Resuelve el servicio y ejecuta
        var dbContext = host.Services.GetRequiredService<ApplicationDbContext>();

        // Aquí puedes usar dbContext para tus operaciones
        Console.WriteLine("Database connection successfully configured!");

        // Mantener la consola abierta
        Console.ReadLine();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped<EFRepository>();
            });
}
