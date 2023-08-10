using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitListener.Application.Interfaces;
using RabbitListener.Application.Services;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var urlProcessor = services.GetRequiredService<IUrlProcessor>();

            var rabbitListener = new RabbitListener.Infrastructure.Messaging.RabbitListener(urlProcessor);
            rabbitListener.StartListening();

            Console.WriteLine("RabbitListener is listening...");
            Console.ReadLine();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<IUrlProcessor, UrlProcessor>();
            });
}
