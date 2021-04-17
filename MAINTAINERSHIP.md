ChangelogAutomation Maintainership
==================================

Release
-------

To release a new version:
1. Update the copyright year in [the license file][license], if required.
2. Choose the new version according to [Semantic Versioning][semver]. It should consist of three numbers (e.g. `1.0.0`).
3. Update the `<Version>` element content in the `Directory.Build.props` file.
4. Make sure there's a properly formed version entry in [the changelog][changelog].
5. Merge the changes via a pull request.
6. Push a tag named `v<VERSION>` to GitHub.

[changelog]: ./CHANGELOG.md
[license]: ./LICENSE.md
[semver]: https://semver.org/spec/v2.0.0.html
