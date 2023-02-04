﻿param (
    $SolutionRootPath = "$PSScriptRoot/..",
    $TemporaryDataPath = "$PSScriptRoot/obj",
    $PackageSourcePath = "$TemporaryDataPath/nuget",
    $NuGetCachePath = "$TemporaryDataPath/nuget.cache",
    $ExtractedNugetPath = "$TemporaryDataPath/nuget.exploded",
    $TestProjectFiles = @("$PSScriptRoot/SingleFrameworkTest.csproj", "$PSScriptRoot/MultiFrameworkTest.csproj"),
    $TestProgramFile = "$PSScriptRoot/Program.cs",
    $dotnet = 'dotnet'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$msBuildTaskProject = "$SolutionRootPath/ChangelogAutomation.MSBuild/ChangelogAutomation.MSBuild.csproj"

Write-Output "Cleaning directory $TemporaryDataPath"
Remove-Item $TemporaryDataPath -Recurse -ErrorAction SilentlyContinue

$previousNuGetPackages = $env:NUGET_PACKAGES
try {
    Write-Output "NUGET_PACKAGES = $NuGetCachePath"
    $env:NUGET_PACKAGES = $NuGetCachePath

    Write-Output 'Packaging ChangelogAutomation.MSBuild'
    & $dotnet pack --configuration Release --output $PackageSourcePath $msBuildTaskProject
    if (!$?) { throw "dotnet pack returned $LASTEXITCODE" }

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
        & $dotnet add $testProjectCsprojPath package --source $PackageSourcePath ChangelogAutomation.MSBuild
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
