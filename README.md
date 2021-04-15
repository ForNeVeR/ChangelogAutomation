ChangelogAutomation [![Status Enfer][status-enfer]][andivionian-status-classifier]
===================

This is a tool for processing of [changelog files][keep-a-changelog].

ChangelogAutomation will extract the first second-level section from a Markdown file, which is useful for extraction of the latest changes section from a changelog.

"Second-level section" is a section preceded by a level 2 header; any of these:

```markdown
## Level 2 header
…some section content…

Also level 2 header
-------------------
…some section content…
```

Prerequisites
-------------

Since ChangelogAutomation is distributed as a self-contained .NET 5 application, it requires the .NET 5 dependencies to be available on the user machine to run. See more information on the dependencies for various operating systems:

- [Linux][deps.linux] (see distribution-specific sections for dependencies)
- [Windows][deps.windows]
- [macOS][deps.macos]

Installation
------------

1. Download the latest stable distribution binaries for your operating system from [GitHub releases page][releases].
2. Unpack the archive, and run `ChangelogAutomation` (or `ChangelogAutomation.exe`) binary from it.

Usage
-----

This invocation will extract the first second-level section of the file, and write it to the output (either the standard one, or an optional output file):

```console
$ ChangelogAutomation <path-to-input-file> [options…]
```

The available options are:

- `(-o|-outputFilePath) <output-file-path>`
- `(-t|-contentType) (Markdown|PlainText)`: output content type

Development
-----------

ChangelogAutomation is a .NET 5 application, so it requires [.NET 5 SDK][dotnet-sdk] (or a later version) for build.

To build the application, run the following command:

```console
$ dotnet build --configuration Release
```

To run the unit test suite, run the following command:

```console
$ dotnet test --configuration Release
```

To run the developer version, execute the following command (usual application arguments follow after `--`):

```console
$ dotnet run --project ChangelogAutomation -- …
```

Documentation
-------------

- [Changelog][changelog]
- [Maintainership][maintainership]
- [License][license] (MIT)

[andivionian-status-classifier]: https://github.com/ForNeVeR/andivionian-status-classifier#status-enfer-
[changelog]: ./CHANGELOG.md
[deps.linux]: https://docs.microsoft.com/en-us/dotnet/core/install/linux
[deps.macos]: https://docs.microsoft.com/en-us/dotnet/core/install/macos#dependencies
[deps.windows]: https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net50#dependencies
[dotnet-sdk]: https://dotnet.microsoft.com/
[keep-a-changelog]: http://keepachangelog.com/
[license]: ./LICENSE.md
[maintainership]: ./MAINTAINERSHIP.md
[releases]: https://github.com/ForNeVeR/ChangelogAutomation/releases

[status-enfer]: https://img.shields.io/badge/status-enfer-orange.svg
