using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ChangelogAutomation.Core;
using ConsoleAppFramework;
using JetBrains.Annotations;

namespace ChangelogAutomation;

[UsedImplicitly]
public class Application
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

    /// <param name="inputFilePath">Path to the input Markdown file</param>
    /// <param name="outputFilePath">-o|Output file path. Will use the standard output if not specified.</param>
    /// <param name="contentType">-t|Output type, either Markdown or PlainText.</param>
    [UsedImplicitly]
    public async Task Run(
        [Argument] string inputFilePath,
        string? outputFilePath = null,
        ContentType contentType = ContentType.Markdown)
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

        if (outputFilePath is null)
        {
            await Console.Out.WriteLineAsync(text);
        }
        else
        {
            await CreateReleaseNotesAsync(text, outputFilePath);
        }
    }

    private static async Task CreateReleaseNotesAsync(string text, string outputFilePath)
    {
        var directoryPath = Path.GetDirectoryName(outputFilePath);

        if (directoryPath is not null && !Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        await using var outputFile = new FileStream(outputFilePath, FileMode.CreateNew);
        await using var writer = new StreamWriter(outputFile);

        await writer.WriteLineAsync(text);
    }
}
