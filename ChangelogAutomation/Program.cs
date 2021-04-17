using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChangelogAutomation
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .RunConsoleAppFrameworkAsync<Application>(args, new ConsoleAppOptions
                {
                    StrictOption = true
                });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<MarkdownToMarkdownConverter>()
                .AddSingleton<MarkdownToPlainTextConverter>();
        }
    }
}
