<!--
SPDX-FileCopyrightText: 2021-2026 Friedrich von Never <friedrich@fornever.me>

SPDX-License-Identifier: MIT
-->

Changelog
=========

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.0.4] - 2026-01-10
### Changed
- Update the dependency versions.

  This includes bumping MSBuild dependencies to 17.8.43 to get rid of build-time warnings about security issues in older MSBuild versions.

## [3.0.3] - 2025-04-23
### Fixed
- [#10: Tool should automatically create the output directory if it doesn't exist](https://github.com/ForNeVeR/ChangelogAutomation/issues/10). Thanks to @y0ung3r!

## [3.0.2] - 2025-04-21
### Changed
- Downgrade required MSBuild to 17.8.3 to support the oldest supported .NET SDK.

## [3.0.1] - 2025-04-19
### Changed
- Migrate the MSBuild package to .NET Standard 2.0 for better compatibility with older SDKs.

## [3.0.0] - 2025-04-19
### Changed
- **(Breaking change.)** Migrate to a more standard notation for arguments:
  - `--outputFilePath` is now `--output-file-path`,
  - `--contentType` is now `--content-type`.
- **(Requirement change.)** Update from .NET 6 to .NET 9.
- The MSBuild package is now treated as a development dependency by default.
- Update dependencies.

### Added
- ChangelogAutomation is now available as a .NET tool. Install **ChangelogAutomation.Tool** package for that. Thanks to @y0ung3r!

## [2.0.0] - 2023-02-04
### Changed
- **(Breaking change.)** Updated from .NET 5 to .NET 6.

### Added
- [#14: Multi-target support for MSBuild](https://github.com/ForNeVeR/ChangelogAutomation/issues/14).
- Ability to run on newer target frameworks than the tool is built for (aka `RollForward = Major`).

## [1.0.1] - 2021-04-25
### Fixed
- Executable bits are now preserved in the release distributions for the console tool version.

## [1.0.0] - 2021-04-25
This is the initial release of the program. It is available as a standalone tool or an MSBuild integration package.

### Added
- Markdown-to-Markdown transformation.
- Markdown-to-plain-text transformation.
- Ability to save the output into a file or to the standard output.

[1.0.0]: https://github.com/ForNeVeR/ChangelogAutomation/releases/tag/v1.0.0
[1.0.1]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v1.0.0...v1.0.1
[2.0.0]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v1.0.1...v2.0.0
[3.0.0]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v2.0.0...v3.0.0
[3.0.1]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v3.0.0...v3.0.1
[3.0.2]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v3.0.1...v3.0.2
[3.0.3]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v3.0.2...v3.0.3
[3.0.3]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v3.0.3...v3.0.4
[Unreleased]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v3.0.4...HEAD
