Changelog
=========

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased] (2.0.0)
### Changed
- **(Breaking change.)** Updated from .NET 5 to .NET 6.

### Added
- [#14: Multi-target support for MSBuild](https://github.com/ForNeVeR/ChangelogAutomation/issues/14)
- Ability to run on newer target frameworks than the tool is built for (aka `RollForward = Major`)

## [1.0.1] - 2021-04-25
### Fixed
- Executable bits are now preserved in the release distributions for the console tool version

## [1.0.0] - 2021-04-25
This is the initial release of the program. It is available as a standalone tool or an MSBuild integration package.

### Added
- Markdown-to-Markdown transformation
- Markdown-to-plain-text transformation
- Ability to save the output into a file or to the standard output

[1.0.0]: https://github.com/ForNeVeR/ChangelogAutomation/releases/tag/v1.0.0
[1.0.1]: https://github.com/ForNeVeR/ChangelogAutomation/compare/v1.0.0...v1.0.1
