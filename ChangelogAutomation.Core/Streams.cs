// SPDX-FileCopyrightText: 2021-2025 Friedrich von Never <friedrich@fornever.me>
//
// SPDX-License-Identifier: MIT

using System;
using System.IO;

namespace ChangelogAutomation.Core
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

    public readonly struct PlainTextStream : IDisposable
    {
        public readonly Stream Stream;

        public PlainTextStream(Stream stream)
        {
            Stream = stream;
        }

        public void Dispose() => Stream.Dispose();
    }
}
