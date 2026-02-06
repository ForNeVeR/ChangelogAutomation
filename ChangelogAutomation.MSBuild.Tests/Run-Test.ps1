# SPDX-FileCopyrightText: 2021-2026 Friedrich von Never <friedrich@fornever.me>
#
# SPDX-License-Identifier: MIT

param (
    $SolutionRootPath = "$PSScriptRoot/..",
    $TemporaryDataPath = "$PSScriptRoot/obj",
    $PackageSourcePath = "$TemporaryDataPath/nuget",
    $NuGetCachePath = "$TemporaryDataPath/nuget.cache",
    $ExtractedNugetPath = "$TemporaryDataPath/nuget.exploded",
    $TestProjectFiles = @(
        "$PSScriptRoot/GeneratePackageOnBuild.csproj",
        "$PSScriptRoot/MultiFrameworkTest.csproj",
        "$PSScriptRoot/SingleFrameworkTest.csproj"
    ),
    $TestProgramFile = "$PSScriptRoot/Program.cs",
    $dotnet = 'dotnet',

    $VisualStudioLocation = 'C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE',
    [switch] $WithVisualStudioIntegration,

    $DevEnv = "$VisualStudioLocation/devenv.com"
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$msBuildTaskProject = "$SolutionRootPath/ChangelogAutomation.MSBuild/ChangelogAutomation.MSBuild.csproj"

Write-Output "Cleaning directory $TemporaryDataPath"
Remove-Item $TemporaryDataPath -Recurse -ErrorAction SilentlyContinue

function Get-TestPackageVersion {
    $directoryBuildPropsFile = "$SolutionRootPath/Directory.Build.props"
    [xml] $directoryBuildProps = Get-Content -Raw $directoryBuildPropsFile
    $version = $directoryBuildProps.SelectSingleNode("//Version").'#text'
    "$version-test"
}

$previousNuGetPackages = $env:NUGET_PACKAGES
try {
    Write-Output "NUGET_PACKAGES = $NuGetCachePath"
    $env:NUGET_PACKAGES = $NuGetCachePath

    $packageVersion = Get-TestPackageVersion

    Write-Output "Packaging ChangelogAutomation.MSBuild $packageVersion…"
    & $dotnet pack --configuration Release --output $PackageSourcePath -p:Version=$packageVersion $msBuildTaskProject
    if (!$?) { throw "dotnet pack returned $LASTEXITCODE" }

    Write-Output 'Emitting a nuget.config file…'
    @"
<configuration>
    <packageSources>
        <add key="Test Repository" value="$PackageSourcePath"/>
    </packageSources>
</configuration>
"@ > "$TemporaryDataPath/nuget.config"

    foreach ($testProjectFileOriginal in $TestProjectFiles) {
        $testProjectName = [IO.Path]::GetFileNameWithoutExtension($testProjectFileOriginal)
        Write-Output "Running test for $testProjectName…"

        $testProjectPath = "$TemporaryDataPath/$testProjectName"
        $testProjectCsprojPath = "$testProjectPath/$testProjectName.csproj"
        $testProjectChangelogPath = "$testProjectPath/../CHANGELOG.md"
        $testProjectNugetPath = "$testProjectPath/nuget"

        Write-Output "Creating a test project in $testProjectPath…"
        New-Item -Type Directory $testProjectPath | Out-Null
        Copy-Item -LiteralPath $testProjectFileOriginal $testProjectCsprojPath
        Copy-Item -LiteralPath $TestProgramFile $testProjectPath

        Write-Output 'Adding a NuGet package…'
        & $dotnet add $testProjectCsprojPath `
            package ChangelogAutomation.MSBuild `
            --version $packageVersion
        if (!$?) { throw "dotnet add returned $LASTEXITCODE." }

        Write-Output "Writing $testProjectChangelogPath…"
        @'
# Changelog

## Version 1.0
- **Test line**
- Another [test][link] line

[unrelated]: https://example.com/unrelated
[link]: https://example.com/
'@ | Out-File $testProjectChangelogPath -Encoding utf8

        if ($WithVisualStudioIntegration) {
            Write-Output "Using `"$DevEnv`" to build project `"$testProjectCsprojPath`"."
            & $DevEnv $testProjectCsprojPath /Build
            if (!$?) { throw "devenv pack returned $LASTEXITCODE." }
        }

        Write-Output "Packing $testProjectName"
        & $dotnet pack `
            --configuration Release `
            --output $testProjectNugetPath `
            $testProjectCsprojPath
        if (!$?) { throw "dotnet pack returned $LASTEXITCODE." }

        $nupkgFile = Get-Item "$testProjectNugetPath/*.nupkg"
        $actualReleaseNotes = & "$PSScriptRoot/Extract-ReleaseNotes.ps1" -NupkgPath $nupkgFile
        $expectedReleaseNotes = @'
- Test line
- Another test (https://example.com/) line
'@.Replace("`r`n", "`n")
        if ($expectedReleaseNotes -ne $actualReleaseNotes) {
            throw "$($testProjectName): release notes not equal to expected. Expected: @'`n$expectedReleaseNotes`n'@`n`nActual: @'`n$actualReleaseNotes`n'@"
        } else {
            Write-Output 'Release notes are equal to expected.'
        }
    }
} finally {
    $env:NUGET_PACKAGES = $previousNuGetPackages
}
