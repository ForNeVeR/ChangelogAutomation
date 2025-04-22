using System.IO;
using System.Text;
using NUnit.Framework;

namespace ChangelogAutomation.Core.Tests;

public static class TestFramework
{
    private static string NormalizeLineEndings(this string t) => t.Replace("\r\n", "\n");

    public static MarkdownStream Markdown(string text) =>
        new(new MemoryStream(Encoding.UTF8.GetBytes(text.NormalizeLineEndings())));

    public static void AssertMarkdownEqual(string expected, MarkdownText actual)
    {
        Assert.That(expected.NormalizeLineEndings(), Is.EqualTo(actual.Content));
    }

    public static void AssertPlainTextEqual(string expected, PlainText actual)
    {
        Assert.That(expected.NormalizeLineEndings(), Is.EqualTo(actual.Content));
    }
}
