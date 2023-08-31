using BotFramework;
using Microsoft.Extensions.Hosting;

namespace CompilerTelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTelegramBot();
            });
    }
}