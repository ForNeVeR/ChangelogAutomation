using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppFramework;
using JetBrains.Annotations;

namespace ChangelogAutomation
{
    [UsedImplicitly]
    public class Application : ConsoleAppBase
    {
        private readonly MarkdownConverter _converter;

        public Application(MarkdownConverter converter)
        {
            _converter = converter;
        }

        [UsedImplicitly]
        public async Task Run(
            [Option(0, "Path to the input Markdown file")] string inputFilePath,
            [Option("o", "Output file path")] string? outputFilePath = null)
        {
            Console.OutputEncoding = Encoding.UTF8;

            await using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            using var inputMdStream = new MarkdownStream(inputStream);
            var text = (await _converter.ExtractVersionSection(inputMdStream)).Text;
            if (outputFilePath == null)
                await Console.Out.WriteLineAsync(text);
            else
            {
                await using var outputFile = new FileStream(outputFilePath, FileMode.CreateNew);
                await using var writer = new StreamWriter(outputFile);

                await writer.WriteLineAsync(text);
            }
        }
    }
}
