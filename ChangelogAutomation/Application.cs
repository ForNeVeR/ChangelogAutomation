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
        private readonly MarkdownToMarkdownConverter _markdownConverter;
        private readonly MarkdownToPlainTextConverter _plainTextConverter;

        public Application(
            MarkdownToMarkdownConverter markdownConverter,
            MarkdownToPlainTextConverter plainTextConverter)
        {
            _markdownConverter = markdownConverter;
            _plainTextConverter = plainTextConverter;
        }

        public enum ContentType
        {
            Markdown,
            PlainText
        }

        [UsedImplicitly]
        public async Task Run(
            [Option(0, "Path to the input Markdown file")] string inputFilePath,
            [Option("o", "Output file path")] string? outputFilePath = null,
            [Option("t", "Output type, either Markdown or PlainText")] ContentType contentType = ContentType.Markdown)
        {
            Console.OutputEncoding = Encoding.UTF8;

            await using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            using var inputMdStream = new MarkdownStream(inputStream);
            var text = contentType switch
            {
                ContentType.PlainText =>
                    (await _plainTextConverter.ExtractVersionSectionPlainText(inputMdStream)).Content,
                _ => (await _markdownConverter.ExtractVersionSection(inputMdStream)).Content
            };

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
