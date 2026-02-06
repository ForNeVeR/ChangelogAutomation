<!--
SPDX-FileCopyrightText: 2024-2026 Friedrich von Never <friedrich@fornever.me>

SPDX-License-Identifier: MIT
-->

Contributor Guide
=================

Development
-----------
ChangelogAutomation is a .NET 9 application, so it requires [.NET 9 SDK][dotnet-sdk] (or a later version) for build.

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
$ pwsh ./ChangelogAutomation.MSBuild.Tests/Run-Test.ps1 [-WithVisualStudioIntegration]
```

Pass the switch `-WithVisualStudioIntegration` if required.

To verify the NuGet package produced for ChangelogAutomation.MSBuild, run the following command:

```console
$ pwsh ./ChangelogAutomation.MSBuild.Tests/Run-PackageValidation.ps1
```

To run the developer version, execute the following command (usual application arguments follow after `--`):

```console
$ dotnet run --project ChangelogAutomation -- …
```

File Encoding Changes
---------------------
If the automation asks you to update the file encoding (line endings or UTF-8 BOM) in certain files, run the following PowerShell script ([PowerShell Core][powershell] is recommended to run this script):
```console
$ pwsh -c "Install-Module VerifyEncoding -Repository PSGallery -RequiredVersion 2.2.1 -Force && Test-Encoding -AutoFix"
```

The `-AutoFix` switch will automatically fix the encoding issues, and you'll only need to commit and push the changes.

License Automation
------------------
If the CI asks you to update the file licenses, follow one of these:
1. Update the headers manually (look at the existing files), something like this:
   ```fsharp
   // SPDX-FileCopyrightText: %year% %your name% <%your contact info, e.g. email%>
   //
   // SPDX-License-Identifier: MIT
   ```
   (accommodate to the file's comment style if required).
2. Alternately, use the [REUSE][reuse] tool:
   ```console
   $ reuse annotate --license MIT --copyright '%your name% <%your contact info, e.g. email%>' %file names to annotate%
   ```

(Feel free to attribute the changes to "ChangelogAutomation contributors <https://github.com/ForNeVeR/ChangelogAutomation>" instead of your name in a multi-author file, or if you don't want your name to be mentioned in the project's source: this doesn't mean you'll lose the copyright.)

[powershell]: https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell
[reuse]: https://reuse.software/
