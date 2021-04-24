ChangelogAutomation Maintainership
==================================

Release
-------

To release a new version:
1. Update the copyright year in [the license file][license], if required.
2. Update the Markdig license link in `DownloadMarkdigLicense.targets`, if the license needs to be updated.
3. Choose the new version according to [Semantic Versioning][semver]. It should consist of three numbers (e.g. `1.0.0`).
4. Update the `<Version>` element content in the `Directory.Build.props` file.
5. Make sure there's a properly formed version entry in [the changelog][changelog].
6. Merge the changes via a pull request.
7. Push a tag named `v<VERSION>` to GitHub.

[changelog]: ./CHANGELOG.md
[license]: ./LICENSE.md
[semver]: https://semver.org/spec/v2.0.0.html
