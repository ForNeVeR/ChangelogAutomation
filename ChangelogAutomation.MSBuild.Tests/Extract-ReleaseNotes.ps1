param (
    [Parameter(Mandatory = $true)]
    [string] $NupkgPath,

    $TemporaryStorage = [IO.Path]::GetTempFileName()
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Remove-Item $TemporaryStorage
$null = New-Item -Type Directory $TemporaryStorage
Expand-Archive -Path $NupkgPath -DestinationPath $TemporaryStorage -Force -ErrorAction 'Stop'

$nuspecFile = Get-Item "$TemporaryStorage/*.nuspec"
[xml] $xmlContent = Get-Content $nuspecFile

return $xmlContent.package.metadata.releaseNotes.Replace("`r`n", "`n")
