# SPDX-FileCopyrightText: 2023-2025 Friedrich von Never <friedrich@fornever.me>
#
# SPDX-License-Identifier: MIT

param(
    [string] $RefName
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Write-Host "Determining version from ref `"$RefName`"…"
if ($RefName -match '^refs/tags/v') {
    $version = $RefName -replace '^refs/tags/v', ''
    Write-Host "Pushed ref is a version tag, version: $version"
} else {
    $propsFilePath = "$PSScriptRoot/../Directory.Build.props"
    [xml] $props = Get-Content $propsFilePath
    foreach ($group in $props.Project.PropertyGroup) {
        if ($group.Label -eq 'Versioning') {
            $version = $group.Version
            break
        }
    }
    Write-Host "Pushed ref is a not version tag, get version from $($propsFilePath): $version"
}

Write-Output $version
