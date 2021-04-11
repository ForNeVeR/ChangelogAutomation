ChangelogAutomation [![Status Enfer][status-enfer]][andivionian-status-classifier]
===================

This is a tool for processing of [changelog files][keep-a-changelog].

ChangelogAutomation will extract the first second-level section from a Markdown file, which is useful for extraction of the latest changes section from a changelog.

"Second-level section" is a level 2 header; any of these:

```markdown
## Level 2 header
…some section content…

Also level 2 header
-------------------
…some section content…
```

Usage
-----

This invocation will extract the first second-level section of the file, and write it to the output (either the standard one, or an optional output file):

```console
$ ChangelogAutomation <path-to-input-file> [options…]
```

The available options are:

- `-o|-outputFilePath <output-file-path>`

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
- [License][license] (MIT)

[andivionian-status-classifier]: https://github.com/ForNeVeR/andivionian-status-classifier#status-enfer-
[changelog]: ./CHANGELOG.md
[dotnet-sdk]: https://dotnet.microsoft.com/
[keep-a-changelog]: http://keepachangelog.com/
[license]: ./LICENSE.md

[status-enfer]: https://img.shields.io/badge/status-enfer-orange.svg
