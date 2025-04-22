using System.IO;
using System.Threading.Tasks;
using ChangelogAutomation.Core;
using NUnit.Framework;
using static ChangelogAutomation.Core.Tests.TestFramework;

namespace ChangelogAutomation.Tests;

public sealed class ApplicationTests
{
    private readonly TempFilePool _tempFilePool = new();
    private readonly Application _sut = new(
        new MarkdownToMarkdownConverter(),
        new MarkdownToPlainTextConverter());

    [Test]
    [TestCase(@"Directory\release-notes.md")]
    [TestCase(@"Directory\Subdirectory\release-notes.md")]
    [TestCase("release-notes.md")]
    public async Task Should_create_release_notes_in_output_directory_properly(string releaseNotesPath)
    {
        // Arrange
        var tempRootPath = Path.Combine(Path.GetTempPath(), nameof(ApplicationTests));
        var outputFilePath = Path.Combine(tempRootPath, releaseNotesPath);

        try
        {
            using var changelogContent = Markdown(
                """
                # Changelog
                ## 2.0
                - Foo
                ## 1.0
                - Bar
                """);

            var changelogPath = await _tempFilePool.CreateTempFileAsync(changelogContent.Stream);

            // Act
            await _sut.Run(changelogPath, outputFilePath);

            // Assert
            var fileInfo = new FileInfo(outputFilePath);
            Assert.That(fileInfo.Exists);
            Assert.That(fileInfo.Length > 0);
        }
        finally
        {
            if (Directory.Exists(tempRootPath))
                Directory.Delete(tempRootPath, recursive: true);
        }
    }

    [TearDown]
    public void TearDown()
        => _tempFilePool.Dispose();
}
