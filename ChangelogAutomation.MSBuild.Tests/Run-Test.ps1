param (
    [switch] $CleanBeforeRun,

    $SolutionRootPath = "$PSScriptRoot/..",
    $TemporaryDataPath = "$PSScriptRoot/obj",
    $NuGetCachePath = "$TemporaryDataPath/nuget-cache",
    $PackageSourcePath = "$TemporaryDataPath/nuget",
    $TestProjectName = 'TestProject',
    $dotnet = 'dotnet'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$msBuildTaskProject = "$SolutionRootPath/ChangelogAutomation.MSBuild/ChangelogAutomation.MSBuild.csproj"

if ($CleanBeforeRun) {
    Write-Output "Cleaning directory $TemporaryDataPath"
    Remove-Item $TemporaryDataPath -Recurse -ErrorAction SilentlyContinue
}

$previousNuGetPackages = $env:NUGET_PACKAGES
try {
    Write-Output "NUGET_PACKAGES = $NuGetCachePath"
    $env:NUGET_PACKAGES = $NuGetCachePath

    Write-Output 'Packaging ChangelogAutomation.MSBuild'
    & $dotnet pack --configuration Release --output $PackageSourcePath $msBuildTaskProject
    if (!$?) { throw "dotnet pack returned $LASTEXITCODE" }

    $testProjectPath = "$TemporaryDataPath/$TestProjectName"
    $testProjectCsprojPath = "$testProjectPath/$TestProjectName.csproj"
    $testProjectChangelogPath = "$testProjectPath/CHANGELOG.md"
    $testProjectNugetPath = "$testProjectPath/nuget"

    Write-Output "Creating a new test project in $testProjectPath"
    & $dotnet new console --output $testProjectPath
    if (!$?) { throw "dotnet new returned $LASTEXITCODE" }

    Write-Output 'Adding a NuGet package'
    & $dotnet add $testProjectCsprojPath package --source $PackageSourcePath ChangelogAutomation.MSBuild
    if (!$?) { throw "dotnet add returned $LASTEXITCODE" }

    @'
    # Changelog

    ## Version 1.0
    - Test line
    - Another test line

    [link]: unrelated test link
'@ | Out-File $testProjectChangelogPath -Encoding utf8

    Write-Output "Packing $TestProjectName"
    & $dotnet pack --no-restore --configuration Release --output $testProjectNugetPath $testProjectCsprojPath
    if (!$?) { throw "dotnet pack returned $LASTEXITCODE" }
} finally {
    $env:NUGET_PACKAGES = $previousNuGetPackages
}
