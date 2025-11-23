// SPDX-FileCopyrightText: 2021-2025 Friedrich von Never <friedrich@fornever.me>
//
// SPDX-License-Identifier: MIT

using System.Threading.Tasks;
using ChangelogAutomation.Core;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;

namespace ChangelogAutomation
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var app = ConsoleApp.Create()
                .ConfigureServices(ConfigureServices);

            app.Add<Application>("");
            await app.RunAsync(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<MarkdownToMarkdownConverter>()
                .AddSingleton<MarkdownToPlainTextConverter>();
        }
    }
}
