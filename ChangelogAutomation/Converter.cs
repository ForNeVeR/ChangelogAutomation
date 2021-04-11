﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ChangelogAutomation
{
    public class Converter
    {
        public async Task<MarkdownText> ExtractVersionSection(MarkdownStream changelogMarkdown)
        {
            using var reader = new StreamReader(changelogMarkdown.Stream, leaveOpen: true);
            var text = await reader.ReadToEndAsync();
            var document = Markdown.Parse(text);
            var nodes = ExtractDocumentBlocksDestructively(document).ToList();

            var sectionDocument = new MarkdownDocument();
            foreach (var block in nodes)
                sectionDocument.Add(block);

            return new MarkdownText(RenderToText(sectionDocument));
        }

        private static string RenderToText(MarkdownDocument document)
        {
            using var writer = new StringWriter { NewLine = "\n" };
            var renderer = new NormalizeRenderer(writer);
            renderer.Render(document);
            return writer.ToString();
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
                .Where(def => def is not null);
    }
}
