using System;
using System.IO;
using System.Threading.Tasks;
using ChangelogAutomation.Core;
using JetBrains.Annotations;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace ChangelogAutomation.MSBuild
{
    [UsedImplicitly]
    public class CollectLastChangelogSection : Task
    {
        [Required, PublicAPI]
        public string ChangelogFilePath { get; set; } = null!;

        [PublicAPI]
        public string OutputType { get; set; } = "PlainText";

        [Output, PublicAPI]
        public string? ChangelogSectionContent { get; set; }

        public override bool Execute()
        {
            if (!File.Exists(ChangelogFilePath))
            {
                Log.LogError($"Could not found changelog file {ChangelogFilePath}");
                return false;
            }

            using var inputStream = new FileStream(ChangelogFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var inputMdStream = new MarkdownStream(inputStream);

            if (OutputType.Equals("PlainText", StringComparison.OrdinalIgnoreCase))
                ChangelogSectionContent = GetResult(
                    new MarkdownToPlainTextConverter().ExtractVersionSectionPlainText(inputMdStream));
            else if (OutputType.Equals("Markdown", StringComparison.OrdinalIgnoreCase))
                ChangelogSectionContent = GetResult(
                    new MarkdownToMarkdownConverter().ExtractVersionSection(inputMdStream));
            else
            {
                Log.LogError($"Couldn't parse OutputType = {OutputType}");
                return false;
            }

            return true;
        }

        private static string GetResult(Task<MarkdownText> task) =>
            task.GetAwaiter().GetResult().Content;
        private static string GetResult(Task<PlainText> task) =>
            task.GetAwaiter().GetResult().Content;
    }
}
