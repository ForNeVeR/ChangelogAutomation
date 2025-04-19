using System.IO;
using System.Threading.Tasks;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ChangelogAutomation.Core;

public class MarkdownToPlainTextConverter : MarkdownConverterBase
{
    public async Task<PlainText> ExtractVersionSectionPlainText(MarkdownStream changelogMarkdown)
    {
        var sectionDocument = await ExtractVersionDocument(changelogMarkdown);
        return new PlainText(RenderToPlainText(sectionDocument));
    }

    private static string RenderToPlainText(MarkdownDocument document)
    {
        using var writer = new StringWriter();
        writer.NewLine = "\n";
        var renderer = new NormalizeRenderer(writer);

        var firstBlock = true;
        foreach (var block in document)
        {
            if (!firstBlock) writer.WriteLine();
            firstBlock = false;

            AppendBlock(block, writer, renderer);
        }

        return writer.ToString();
    }

    private static void AppendBlock(Block block, StringWriter writer, NormalizeRenderer renderer)
    {
        switch (block)
        {
            case HeadingBlock heading:
                writer.Write('[');
                if (heading.Inline is { } hInlines)
                {
                    foreach (var item in hInlines)
                        AppendInline(item, writer, renderer);
                }
                writer.WriteLine(']');
                break;

            case QuoteBlock quote:
                foreach (var item in quote)
                {
                    using var nestedWriter = new StringWriter();
                    nestedWriter.NewLine = "\n";
                    var nestedRenderer = new NormalizeRenderer(nestedWriter);
                    AppendBlock(item, nestedWriter, nestedRenderer);

                    var quotedLines = nestedWriter.ToString().Split('\n');
                    for (var i = 0; i < quotedLines.Length; ++i)
                    {
                        var line = quotedLines[i];
                        if (i == quotedLines.Length - 1 && line == "") continue;
                        writer.WriteLine($"> {line}");
                    }
                }
                break;

            case CodeBlock code:
                for (var index = 0; index < code.Lines.Count; ++index)
                    writer.WriteLine(code.Lines.Lines[index]);
                break;

            case ParagraphBlock paragraph:
                if (paragraph.Inline is { } pInlines)
                {
                    foreach (var item in pInlines)
                        AppendInline(item, writer, renderer);
                }
                writer.WriteLine();
                break;

            case ThematicBreakBlock:
                writer.WriteLine("* * *");
                break;

            case ListBlock { BulletType: '1' } list:
                var number = 1;
                foreach (var item in list)
                {
                    writer.Write($"{number++}. ");
                    AppendBlock(item, writer, renderer);
                }
                break;

            case ListBlock list:
                foreach (var item in list)
                {
                    writer.Write("- ");
                    AppendBlock(item, writer, renderer);
                }
                break;

            case LinkReferenceDefinitionGroup _:
                // Handled automatically when processing links; no output is expected.
                break;

            case ContainerBlock container:
                foreach (var item in container)
                    AppendBlock(item, writer, renderer);
                break;

            default:
                renderer.Render(block);
                break;
        }
    }

    private static void AppendInline(Inline inline, StringWriter writer, NormalizeRenderer renderer)
    {
        switch (inline)
        {
            case LineBreakInline:
                writer.Write(' ');
                break;

            case EmphasisInline emphasis:
                foreach (var item in emphasis)
                    AppendInline(item, writer, renderer);
                break;

            case CodeInline code:
                writer.Write(code.Content);
                break;

            case LinkInline link:
                foreach (var item in link)
                    AppendInline(item, writer, renderer);
                writer.Write($" ({link.Url})");
                break;

            default:
                renderer.Render(inline);
                break;
        }
    }
}
