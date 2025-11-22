using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ChangelogAutomation.Core;

public class MarkdownConverterBase
{
    protected async Task<MarkdownDocument> ExtractVersionDocument(MarkdownStream changelogMarkdown)
    {
        var reader = new StreamReader(changelogMarkdown.Stream); // note no dispose because we don't want to own the stream
        var text = await reader.ReadToEndAsync();
        var document = Markdown.Parse(text);
        var nodes = ExtractDocumentBlocksDestructively(document).ToList();

        var sectionDocument = new MarkdownDocument();
        foreach (var block in nodes)
            sectionDocument.Add(block);

        return sectionDocument;
    }

    private static IEnumerable<Block> ExtractDocumentBlocksDestructively(MarkdownDocument document)
    {
        var referenceDefinitions = new HashSet<LinkReferenceDefinition>();

        var sectionBlocks = document
            .SkipWhile(n => n is not HeadingBlock {Level: 2})
            .Skip(1)
            .TakeWhile(n => n is not HeadingBlock {Level: 2});

        foreach (var block in sectionBlocks)
        {
            if (block is LinkReferenceDefinitionGroup) continue;

            foreach (var reference in ExtractReferencedLinks(block))
                referenceDefinitions.Add(reference);

            yield return block;
        }

        document.Clear();

        if (referenceDefinitions.Count <= 0) yield break;

        var referenceBlock = new LinkReferenceDefinitionGroup();
        foreach (var def in referenceDefinitions.OrderBy(d => d.Label))
        {
            def.Parent?.Remove(def);
            referenceBlock.Add(def);
        }

        yield return referenceBlock;
    }

    private static IEnumerable<LinkReferenceDefinition> ExtractReferencedLinks(Block block) =>
        block
            .Descendants<LinkInline>()
            .Select(li => li.Reference)
            .Where(def => def is not null)!;
}
