using System.IO;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ChangelogAutomation
{
    public class MarkdownToPlainTextConverter : MarkdownConverterBase
    {
        public async Task<PlainText> ExtractVersionSectionPlainText(MarkdownStream changelogMarkdown)
        {
            var sectionDocument = await ExtractVersionDocument(changelogMarkdown);
            return new PlainText(RenderToPlainText(sectionDocument));
        }

        private static string RenderToPlainText(MarkdownDocument document)
        {
            using var writer = new StringWriter { NewLine = "\n" };
            var renderer = new NormalizeRenderer(writer);

            var firstBlock = true;
            foreach (var block in document)
            {
                if (!firstBlock) writer.WriteLine();
                firstBlock = false;

                AppendBlock(block);
            }

            return writer.ToString();

            void AppendBlock(Block block)
            {
                switch (block)
                {
                    case CodeBlock code:
                        for (var index = 0; index < code.Lines.Count; ++index)
                            writer.WriteLine(code.Lines.Lines[index]);
                        break;

                    case ParagraphBlock paragraph:
                        foreach (var item in paragraph.Inline)
                            AppendInline(item);
                        break;

                    default:
                        renderer.Render(block);
                        break;
                }
            }

            void AppendInline(Inline inline)
            {
                switch (inline)
                {
                    case EmphasisInline emphasis:
                        foreach (var item in emphasis)
                            AppendInline(item);
                        break;

                    case CodeInline code:
                        writer.Write(code.Content);
                        break;

                    default:
                        renderer.Render(inline);
                        break;
                }
            }
        }
    }
}
