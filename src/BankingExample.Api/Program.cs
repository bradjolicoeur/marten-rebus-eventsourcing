using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Oakton;
using System.Threading.Tasks;

namespace BankingExample.Api
{
    public class Program
    {
        public static Task<int> Main(string[] args)
        {
            return CreateHostBuilder(args)

            // This line replaces Build().Start()
            // in most dotnet new templates
            .RunOaktonCommands(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
