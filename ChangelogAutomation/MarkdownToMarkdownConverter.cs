using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;

namespace ChangelogAutomation
{
    public class MarkdownToMarkdownConverter : MarkdownConverterBase
    {
        public async Task<MarkdownText> ExtractVersionSection(MarkdownStream changelogMarkdown)
        {
            var sectionDocument = await ExtractVersionDocument(changelogMarkdown);
            return new MarkdownText(RenderToString(sectionDocument));
        }

        private static string RenderToString(MarkdownDocument document)
        {
            using var writer = new StringWriter { NewLine = "\n" };
            var renderer = new NormalizeRenderer(writer);
            renderer.Render(document);
            return writer.ToString();
        }
    }
}
