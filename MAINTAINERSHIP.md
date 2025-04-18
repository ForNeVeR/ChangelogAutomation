Maintainer Guide
================

Publish a New Version
---------------------
To release a new version:
1. Update the project's status in the `README.md` file, if required.
2. Update the copyright year in [the license file][license], if required.
3. Update the Markdig license information in the `Directory.Build.props`, if required.
4. Choose the new version according to [Semantic Versioning][semver]. It should consist of three numbers (e.g. `1.0.0`).
5. Update the `<Version>` element content in the `Directory.Build.props` file.
6. Make sure there's a properly formed version entry in [the changelog][changelog].
7. Merge the changes via a pull request.
8. Check if the NuGet key is still valid (see the **Rotate NuGet Publishing Key** section if it isn't).
9. Push a tag named `v<VERSION>` to GitHub.

Rotate NuGet Publishing Key
---------------------------
CI relies on the NuGet API key being added to the secrets.
From time to time, this key requires maintenance: it will become obsolete and will have to be updated.

To update the key:

1. Sign in onto nuget.org.
2. Go to the [API keys][nuget.api-keys] section.
3. Update the existing or create a new key named `changelogautomation.github` with a permission to **Push only new package versions** and only allowed to publish the packages **ChangelogAutomation.MSBuild** and **ChangelogAutomation.Tool**.

   (If this is the first publication of a new package,
   upload a temporary short-living key with permission to add new packages and rotate it afterward.)
4. Paste the generated key to the `NUGET_TOKEN` variable on the [action secrets][github.secrets] section of GitHub settings.

[changelog]: ./CHANGELOG.md
[github.secrets]: https://github.com/ForNeVeR/ChangelogAutomation/settings/secrets/actions
[license]: ./LICENSE.md
[nuget.api-keys]: https://www.nuget.org/account/apikeys
[semver]: https://semver.org/spec/v2.0.0.html
