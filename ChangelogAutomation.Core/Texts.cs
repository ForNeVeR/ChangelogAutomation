namespace ChangelogAutomation.Core
{
    public readonly struct MarkdownText
    {
        public readonly string Content;

        public MarkdownText(string content)
        {
            Content = content;
        }
    }

    public readonly struct PlainText
    {
        public readonly string Content;

        public PlainText(string text)
        {
            Content = text;
        }
    }
}
