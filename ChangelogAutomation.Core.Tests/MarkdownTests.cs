using System.Threading.Tasks;
using NUnit.Framework;
using static ChangelogAutomation.Core.Tests.TestFramework;

namespace ChangelogAutomation.Core.Tests
{
    public class Tests
    {
        private readonly MarkdownToMarkdownConverter _converter = new();

        private async Task ExtractionTest(string input, string expectedOutput)
        {
            using var source = Markdown(input);
            var result = await _converter.ExtractVersionSection(source);
            AssertMarkdownEqual(expectedOutput, result);
        }

        [Test]
        public Task SimpleExtractionTest() => ExtractionTest(@"# Changelog
## 2.0
- Foo
## 1.0
- Bar
", @"- Foo");

        [Test]
        public Task InlineLinkTest() => ExtractionTest(@"# Changelog
## 2.0
- [Foo](https://example.com/foo)
- [Bar](https://example.com/foo)
## 1.0
- [Bar][link]

[link]: https://example.com
", @"- [Foo](https://example.com/foo)
- [Bar](https://example.com/foo)");

        [Test]
        public Task LinkReferenceExtractionTest() => ExtractionTest(@"# Changelog
## 2.0
- [Foo][link]
## 1.0
- Bar

[link]: https://example.com
", @"- [Foo][link]

[link]: https://example.com");

        [Test]
        public Task LinkBlockIgnoreTest() => ExtractionTest(@"# Changelog
## 1.0
- [Bar][link]

[link]: https://example.com
[Unreleased]: https://example.com/2
", @"- [Bar][link]

[link]: https://example.com");

        [Test]
        public Task MultipleLinksToTheSameTest() => ExtractionTest(@"# Changelog
## 1.0
- [Bar][link]
- [Baz][link]

[link]: https://example.com
[Unreleased]: https://example.com/2
", @"- [Bar][link]
- [Baz][link]

[link]: https://example.com");

        [Test]
        public Task OtherFormatOfLevel2HeaderTest() => ExtractionTest(@"
Section 1
---------
Section 1 body

Section 2
---------
Section 2 body
", @"Section 1 body");
    }
}
