using System;
using System.IO;

namespace ChangelogAutomation
{
    public readonly struct MarkdownStream : IDisposable
    {
        public readonly Stream Stream;

        public MarkdownStream(Stream stream)
        {
            Stream = stream;
        }

        public void Dispose() => Stream.Dispose();
    }
}
