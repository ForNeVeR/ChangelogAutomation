using System.Threading.Tasks;
using NUnit.Framework;
using static ChangelogAutomation.Tests.TestFramework;

namespace ChangelogAutomation.Tests
{
    public class PlainTextTests
    {
        private readonly MarkdownToPlainTextConverter _converter = new();

        private async Task ConversionTest(string input, string expectedOutput)
        {
            using var source = Markdown(input);
            var result = await _converter.ExtractVersionSectionPlainText(source);
            AssertPlainTextEqual(expectedOutput, result);
        }

        [Test]
        public Task ParagraphsTest() =>
            ConversionTest(@"# Changelog
## Version 1.0
Paragraph 1

Paragraph 2

Paragraph
text
test", @"Paragraph 1

Paragraph 2

Paragraph text test");

        [Test]
        public Task HeadersTest() =>
            ConversionTest(@"# Changelog
## Version 1.0
### Header
Content

Content

### Header

Content", @"[Header]

Content

Content

[Header]

Content
");

        [Test]
        public Task FormattingTest() =>
            ConversionTest(@"# Changelog
## Version 1.0
**Bold**
test
_italic_ __bold__

_Italic_

---", @"Bold test italic bold

Italic

* * *
");

        [Test]
        public Task LinksTest() =>
            ConversionTest(@"# Changelog
## Version 1.0
[Link 1](https://example.com/1)

[Link 2][link2]

## Version 0.9
[link2]: https://example.com/2", @"Link 1 (https://example.com/1)

Link 2 (https://example.com/2)");

        [Test]
        public Task ListsTest() =>
            ConversionTest(@"# Changelog
## Version 1.0
1. Item 1
1. Item 2

- Item 1
- Item 2", @"1. Item 1
2. Item 2

- Item 1
- Item 2");

        [Test]
        public Task QuoteTest() =>
            ConversionTest(@"# Changelog
## Version 1.0
> Quote
> Quote 2

> Quote **3**
>
> Quote 4", @"> Quote Quote 2

> Quote 3
>
> Quote 4");

        [Test]
        public Task CodeTest() =>
            ConversionTest(@"# Changelog
## Version 1.0
```
Code 1
Code 2
```

    Code 1
    Code 2

Paragraph with `code 3`.", @"Code 1
Code 2

Code 1
Code 2

Paragraph with code 3.
");
    }
}
