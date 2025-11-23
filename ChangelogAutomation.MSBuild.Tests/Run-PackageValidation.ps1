# SPDX-FileCopyrightText: 2021-2025 Friedrich von Never <friedrich@fornever.me>
#
# SPDX-License-Identifier: MIT

param (
    $NupkgPath = "$PSScriptRoot/../ChangelogAutomation.MSBuild/bin/Release/*.nupkg",
    $ChangelogAutomationCsproj = "$PSScriptRoot/../ChangelogAutomation/ChangelogAutomation.csproj",
    $ChangelogFilePath = "$PSScriptRoot/../CHANGELOG.md",
    $dotnet = 'dotnet'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function normalize($linesOrString) {
    $lines = $linesOrString.Split("`n")
    ($lines -join "`n").TrimEnd()
}

$actualReleaseNotes = normalize(& "$PSScriptRoot/Extract-ReleaseNotes.ps1" -NupkgPath $NupkgPath)
$expectedReleaseNotes = normalize(
    & $dotnet run `
        --project $ChangelogAutomationCsproj -- `
        $ChangelogFilePath `
        --content-type PlainText
)

if ($expectedReleaseNotes -ne $actualReleaseNotes) {
    throw "Release notes not equal to expected. Expected: @'`n$expectedReleaseNotes`n'@`n`nActual: @'`n$actualReleaseNotes`n'@"
} else {
    Write-Output 'Release notes are equal to expected'
}
