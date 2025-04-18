ChangelogAutomation [![Status Terrid][status-terrid]][andivionian-status-classifier]
===================

When preparing a software release, a common task is to parse [the changelog file][keep-a-changelog], and extract the latest changes section from it. For certain purposes, Markdown format is acceptable, but in some cases a text format is also useful.

ChangelogAutomation is a tool for this exact purpose. It will extract the first second-level section from a Markdown file either in a text or in a Markdown format, and will print it to a file or to its stdout.

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

Since ChangelogAutomation is distributed as a self-contained .NET 9 application, it requires the .NET 9 dependencies (but not the .NET 9 runtime itself) to be available on the user machine to run. See more information [in the documentation][docs.dotnet.install].

Usage
-----

### GitHub Actions

There's a separate repository with GitHub Action integration of this tool. Check out [the documentation][github-actions].

### Console Tool [![NuGet Package][nuget.badge.tool]][nuget.package.tool]

ChangelogAutomation is available for installation in two variants:
- as a .NET tool:
  ```console
  $ dotnet tool install --global ChangelogAutomation.Tool
  ```
- as a standalone native tool for several operating systems on the [GitHub releases page][releases].

Then you can run the tool using:
```console
$ ChangelogAutomation <path-to-input-file> [options…]
```

This invocation will extract the first second-level section of the file and write it to the output
(either the standard output or an optional output file).

The available *options* are:

- `(-o|--output-file-path) <output-file-path>` (if not specified, then will print to stdout)
- `(-t|--content-type) (Markdown|PlainText)`: output content type

### MSBuild [![NuGet Package][nuget.badge.msbuild]][nuget.package.msbuild]

There's an MSBuild task package available. The package will automatically integrate with `dotnet pack`, and extract the latest changelog entry into the `<releaseNotes>` element in the `.nuspec` file.

Just add a package reference to `ChangelogAutomation.MSBuild` package, and the task will automatically be enabled. It will set the `PackageReleaseNotes` property to the latest version section contents (this is a standard property, will then be used by NuGet integration automatically).

There are MSBuild properties to tune its behavior:

- `DisableChangelogAutomationTask`: set this to `true` to disable the automatic task invocation (if you want to register it with the custom parameters).
- `ChangelogFilePath`: point it to the `CHANGELOG.md` file. By default, will be set to `../CHANGELOG.md` (resolved relatively to the project file location).
- `ReleaseNotesOutputType`: either `Markdown` or `PlainText`. If not set, defaults to `PlainText`.

Development
-----------

ChangelogAutomation is a .NET 6 application, so it requires [.NET 6 SDK][dotnet-sdk] (or a later version) for build.

To build the application, run the following command:

```console
$ dotnet build --configuration Release
```

To run the unit test suite, run the following command:

```console
$ dotnet test --configuration Release
```

To run the integration test for the MSBuild task package, run the following command:

```console
$ pwsh ./ChangelogAutomation.MSBuild.Tests/Run-Test.ps1
```

To verify the NuGet package produced for ChangelogAutomation.MSBuild, run the following command:

```console
$ pwsh ./ChangelogAutomation.MSBuild.Tests/Run-PackageValidation.ps1
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

[andivionian-status-classifier]: https://andivionian.fornever.me/v1/#status-terrid-
[changelog]: ./CHANGELOG.md
[docs.dotnet.install]: https://learn.microsoft.com/en-us/dotnet/core/install/
[dotnet-sdk]: https://dotnet.microsoft.com/
[github-actions]: https://github.com/marketplace/actions/changelogautomation-action
[keep-a-changelog]: http://keepachangelog.com/
[license]: ./LICENSE.md
[maintainership]: ./MAINTAINERSHIP.md
[nuget.badge.msbuild]: https://img.shields.io/nuget/v/ChangelogAutomation.MSBuild
[nuget.badge.tool]: https://img.shields.io/nuget/v/ChangelogAutomation.Tool
[nuget.package.msbuild]: https://www.nuget.org/packages/ChangelogAutomation.MSBuild/
[nuget.package.tool]: https://www.nuget.org/packages/ChangelogAutomation.Tool/
[releases]: https://github.com/ForNeVeR/ChangelogAutomation/releases
[status-terrid]: https://img.shields.io/badge/status-terrid-green.svg
