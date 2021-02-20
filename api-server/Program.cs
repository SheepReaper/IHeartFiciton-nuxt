using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Sheep.IHeartFiction.ApiServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builtHost = CreateHostBuilder(args).Build();
            builtHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
