<#
.SYNOPSIS
Generates Linux Core Dumps from all supported .NET containers with a large variety of application types (console/aspnet, release/debug, platform independent/R2R).

Run './GenerateCoreDumps.ps1 -All -OutputFolder <output_folder>' to generate the full matrix.

This is not expected to run in a build definition.
It's supposed to be an utility to easily generate a large number of dumps with similar call stacks.
Dumps generated with this script are also part of Concord Glass tests.
#>

[CmdletBinding(DefaultParameterSetName='SingleConfig')]
param(
    [Parameter(ParameterSetName='SingleConfig', Mandatory = $true)]
    [Parameter(ParameterSetName='All', Mandatory = $false)]
    [ValidateSet('3.1.7', '5.0-preview8')]
    [string]
    $NetCoreVersion,

    [Parameter(ParameterSetName='SingleConfig', Mandatory = $true)]
    [Parameter(ParameterSetName='All', Mandatory = $false)]
    [ValidateSet('console', 'aspnet')]
    [string]
    $TestApplication,

    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]
    $OutputFolder,

    [Parameter(ParameterSetName='SingleConfig', Mandatory = $true)]
    [Parameter(ParameterSetName='All', Mandatory = $false)]
    [ValidateSet('Debug', 'Release')]
    [string]
    $BuildConfiguration,

    [Parameter(Mandatory = $false)]
    [Switch]
    $BuildR2R,

    [Parameter( Mandatory = $false)]
    [Switch]
    $BuildSelfContained,

    [Parameter(Mandatory = $false)]
    [Switch]
    $SkipBuild,

    [Parameter(ParameterSetName='All',Mandatory = $true)]
    [Switch]
    $All
)

$AvailableContainers = @{
    '3.1.7' = @(
        @{Name="3.1.7-buster-slim"; CLib="gnu"; RuntimeId="linux-x64" },
        @{Name="3.1.7-focal"; CLib="gnu"; RuntimeId="linux-x64" },
        @{Name="3.1.7-bionic"; CLib="gnu"; RuntimeId="linux-x64" },
        @{Name="3.1.7-alpine3.12"; CLib="musl"; RuntimeId="linux-musl-x64" },
        @{Name="3.1.7-alpine3.11"; CLib="musl"; RuntimeId="linux-musl-x64" }
        # createdump doesn't work on arm containers
        # @{Name="3.1.7-buster-slim-arm64v8"; CLib="gnu"; RuntimeId="linux-arm64" },
        # @{Name="3.1.7-focal-arm64v8"; CLib="gnu"; RuntimeId="linux-arm64" },
        # @{Name="3.1.7-bionic-arm64v8"; CLib="gnu"; RuntimeId="linux-arm64" },
        # @{Name="3.1.7-alpine3.12-arm64v8"; CLib="musl"; RuntimeId="linux-musl-arm64" },
        # @{Name="3.1.7-alpine3.11-arm64v8"; CLib="musl"; RuntimeId="linux-musl-arm64" },
        # @{Name="3.1.7-buster-slim-arm32v7"; CLib="gnu"; RuntimeId="linux-arm" },
        # @{Name="3.1.7-focal-arm32v7"; CLib="gnu"; RuntimeId="linux-arm" },
        # @{Name="3.1.7-bionic-arm32v7"; CLib="gnu"; RuntimeId="linux-arm" }
    );
    '5.0-preview8' = @(
        @{Name="5.0.0-preview.8-buster-slim"; CLib="gnu"; RuntimeId="linux-x64" },
        @{Name="5.0.0-preview.8-focal"; CLib="gnu"; RuntimeId="linux-x64" },
        @{Name="5.0.0-preview.8-alpine3.12"; CLib="musl"; RuntimeId="linux-musl-x64" }
        # createdump doesn't work on arm containers
        # @{Name="5.0.0-preview.8-buster-slim-arm64v8"; CLib="gnu"; RuntimeId="linux-arm64" },
        # @{Name="5.0.0-preview.8-focal-arm64v8"; CLib="gnu"; RuntimeId="linux-arm64" },
        # @{Name="5.0.0-preview.8-alpine3.12-arm64v8"; CLib="musl"; RuntimeId="linux-musl-arm64" },
        # @{Name="5.0.0-preview.8-buster-slim-arm32v7"; CLib="gnu"; RuntimeId="linux-arm" },
        # @{Name="5.0.0-preview.8-focal-arm32v7"; CLib="gnu"; RuntimeId="linux-arm" }
    )
}

$AvailableApps = @{
    'console' = @{
        '3.1.7'= @{
            AppName='ConsoleApplication';
            TargetFramework='netcoreapp3.1';
            ImageBase='dotnet/core/runtime'
        };
        '5.0-preview8'= @{
            AppName='ConsoleApplication';
            TargetFramework='net5.0';
            ImageBase='dotnet/runtime'
        }
    };
    'aspnet' = @{
        '3.1.7'= @{
            AppName='AppDiagnosticsCore3.1';
            TargetFramework='netcoreapp3.1';
            ImageBase='dotnet/core/aspnet'
        };
        '5.0-preview8'= @{
            AppName='AppDiagnosticsNet5';
            TargetFramework='net5.0';
            ImageBase='dotnet/aspnet'
        }
    }
}

function Build-App
{
    param (
        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $TestAppName,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $BuildConfiguration,

        [Parameter()]
        [string] $RuntimeId = "",

        [Parameter(Mandatory = $false)]
        [Boolean]
        $BuildR2R = $false,

        [Parameter(Mandatory = $false)]
        [Boolean]
        $BuildClean = $false
    )

    Write-Host "`n=== Build-App(app:'$TestAppName' config:'$BuildConfiguration' R2R:'$BuildR2R' RuntimeId:'$RuntimeId') ===`n"

    $SourceAppCsproj = "$PSScriptRoot\$TestAppName\$TestAppName.csproj"


    if ($BuildClean)
    {
        dotnet clean $SourceAppCsproj -c $BuildConfiguration
        Remove-Item -Recurse -Force "$PSScriptRoot\$TestAppName\bin"
        Remove-Item -Recurse -Force "$PSScriptRoot\$TestAppName\obj"
    }

    if (![string]::IsNullOrEmpty($RuntimeId))
    {
        if ($BuildR2R)
        {
            dotnet build $SourceAppCsproj -c $BuildConfiguration -p:PublishReadyToRun=true -p:RuntimeIdentifier=$RuntimeId
        }
        else
        {
            dotnet build $SourceAppCsproj -c $BuildConfiguration -p:RuntimeIdentifier=$RuntimeId
        }
    }
    else
    {
        if ($BuildR2R)
        {
            dotnet build $SourceAppCsproj -c $BuildConfiguration -p:PublishReadyToRun=true
        }
        else
        {
            dotnet build $SourceAppCsproj -c $BuildConfiguration
        }
    }
    if (!$?) {
        throw "dotnet build failed";
    }
}

function Build-DockerImage
{
    param (
        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $TestAppName,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $TestAppFolder,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $ImageBase,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $BaseTag,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $CLib = "gnu",

        [Parameter()]
        [string] $RuntimeId = ""
    )
    Write-Host "`n=== Build-DockerImage(app:'$TestAppName' TestAppFolder:'$TestAppFolder' ImageBase:'$ImageBase' tag:'$BaseTag' CLib:'$CLib' RuntimeId:'$RuntimeId') ===`n"
    
    $TagName = "$TestAppName-$BaseTag-$CLib"

    if ([string]::IsNullOrEmpty($RuntimeId))
    {
        $TagName = "$TestAppName-$BaseTag-$CLib"
    }
    else {
        $TagName = "$TestAppName-$BaseTag-$CLib-$RuntimeId"
        $TestAppFolder = "$TestAppFolder\$RuntimeId"
    }
    $TagName = $TagName.ToLowerInvariant()

    docker build --build-arg BaseTag=$BaseTag --build-arg ImageBase=$ImageBase --build-arg CLib=$CLib -t $TagName $TestAppFolder
    if (!$?) {
        throw "docker build failed";
    }
}

function Invoke-DockerContainer
{
    param (
        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $TestAppName,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $DumpType,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $BaseTag,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $DumpOutputFolder,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $CLib = "gnu",

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $BuildConfiguration,

        [Parameter()]
        [string] $RuntimeId = "",

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [Boolean]$CallCrashEndpoint = $false
    )

    if ([string]::IsNullOrEmpty($RuntimeId))
    {
        $TagName = "$TestAppName-$BaseTag-$CLib"
    }
    else {
        $TagName = "$TestAppName-$BaseTag-$CLib-$RuntimeId"
    }
    $TagName = $TagName.ToLowerInvariant()

    $ContainerName = "$TagName-running"
    docker stop $ContainerName

    $ScenarioName="$BaseTag-$BuildConfiguration".ToLowerInvariant()
    $DockerRunScriptBlock = {
        param($ContainerName, $DumpOutputFolder, $ScenarioName, $DumpType, $TagName)
        docker run --rm --name $ContainerName -p 80:80/tcp -v "$($DumpOutputFolder):/coredumps" -e SCENARIO_NAME=$ScenarioName -e DUMP_TYPE="$DumpType" --privileged --cap-add=SYS_PTRACE ($TagName):latest
    }
    $DockerRunJob = Start-Job -ScriptBlock $DockerRunScriptBlock -ArgumentList @($ContainerName, $DumpOutputFolder, $ScenarioName, $DumpType, $TagName)

    if ($CallCrashEndpoint)
    {
        $CallCrashEndpointScriptBlock = {
            $maxRetries = 15
            while ($maxRetries -gt 0)
            {
                $result = Invoke-WebRequest -Uri "http://localhost/" -ErrorAction Continue
                if ( $result.StatusCode -eq 200)
                {
                    #website is up
                    break
                }
                $maxRetries = $maxRetries - 1
                Start-Sleep -s 1
            }
            Invoke-WebRequest -Uri "http://localhost/Home/Crash" -ErrorAction SilentlyContinue
        }
        Start-Job -ScriptBlock $CallCrashEndpointScriptBlock | Receive-Job -AutoRemoveJob -Wait
    }

    $DockerRunJob | Receive-Job -AutoRemoveJob -Wait
}

function Get-CoreDumps
{
    param (
        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $TestAppName,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $TestAppFolder,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $ImageBase,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $BaseTag,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $OutputFolder,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $CLib = "gnu",

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string] $BuildConfiguration,

        [Parameter()]
        [string] $RuntimeId,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [Boolean]$CallCrashEndpoint = $false
    )

    Write-Host "`n=== Get-CoreDumps(app:'$TestAppName' AppFolder:'$TestAppFolder' tag:'$BaseTag' CLib:'$CLib' RuntimeId:'$RuntimeId' Output:'$OutputFolder') ===`n"

    Build-DockerImage -TestAppName $TestAppName -ImageBase $ImageBase -BaseTag $BaseTag -CLib $CLib -TestAppFolder $TestAppFolder -RuntimeId $RuntimeId

    $dumpTypes =@("default", "normal", "withheap", "triage", "full")

    foreach ($type in $dumpTypes)
    {
        Invoke-DockerContainer -TestAppName $TestAppName -RuntimeId $RuntimeId -DumpOutputFolder $OutputFolder -BuildConfiguration $BuildConfiguration -BaseTag $BaseTag -CLib $CLib -DumpType $type -CallCrashEndpoint $CallCrashEndpoint
    }
}

function Invoke-CoreDumpGeneration
{
    param (
        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string]$NetCoreVersion,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string]$TestApplication,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string]$OutputFolder,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [string]$BuildConfiguration,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [Boolean]$BuildR2R = $false,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [Boolean]$BuildSelfContained = $false,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [Boolean]$SkipBuild = $false
    )

    #setup
    $ContainersInformation = $AvailableContainers[$NetCoreVersion]
    $TestAppName = $AvailableApps[$TestApplication][$NetCoreVersion].AppName
    $ImageBase = $AvailableApps[$TestApplication][$NetCoreVersion].ImageBase
    $TargetFramework = $AvailableApps[$TestApplication][$NetCoreVersion].TargetFramework
    $TestAppFolder = "$PSScriptRoot\$TestAppName\bin\$BuildConfiguration\$TargetFramework"
    $CallCrashEndpoint = ($TestApplication -like "*aspnet*")

    #build
    if (!$SkipBuild)
    {
        if (!$BuildSelfContained)
        {
            Build-App -TestAppName $TestAppName -BuildConfiguration $BuildConfiguration -BuildR2R $BuildR2R -BuildClean $true
        }
        else
        {
            $runtimes = $ContainersInformation | Select-Object -ExpandProperty RuntimeId | Get-Unique -AsString
            $firstLoop = $true
            foreach ($runtimeId in $runtimes)
            {
                Build-App -TestAppName $TestAppName -BuildConfiguration $BuildConfiguration -BuildR2R $BuildR2R  -BuildClean $firstLoop -RuntimeId $runtimeId
                $firstLoop = $false
            }
        }
    }

    #run
    $OutputFolder = "$OutputFolder\$($NetCoreVersion)_$($TestApplication)_$($BuildConfiguration)"
    if ($BuildR2R)
    {
        $OutputFolder = "$($OutputFolder)_R2R"
    }
    if ($BuildSelfContained )
    {
        $OutputFolder = "$($OutputFolder)_contained"
    }
    New-Item -ItemType Directory -Force -Path $OutputFolder

    foreach ($container in $ContainersInformation)
    {
        if (!$BuildSelfContained)
        {
            $runtimeId = ""
        }
        else
        {
            $runtimeId = $container.RuntimeId
        }

        Get-CoreDumps -BaseTag $container.Name -ImageBase $ImageBase -CLib $container.CLib -TestAppFolder $TestAppFolder -TestAppName $TestAppName -RuntimeId $runtimeId -BuildConfiguration $BuildConfiguration -CallCrashEndpoint $CallCrashEndpoint -OutputFolder $OutputFolder
    }
    xcopy $TestAppFolder $OutputFolder /E/H/Y
}

if ($All)
{
    foreach($app in $AvailableApps.Keys)
    {
        if ($PSBoundParameters.ContainsKey('TestApplication') -and $app -ne $TestApplication) { continue }
        foreach($version in $AvailableApps[$app].Keys)
        {
            if ($PSBoundParameters.ContainsKey('NetCoreVersion') -and $version -ne $NetCoreVersion ) { continue }
            foreach($r2r in @($true, $false))
            {
                foreach($selfContained in @($true, $false))
                {
                    foreach($buildConfig in @('Debug', 'Release'))
                    {
                        if ($PSBoundParameters.ContainsKey('BuildConfiguration') -and $buildConfig -ne $BuildConfiguration ) { continue }
                        Invoke-CoreDumpGeneration -TestApplication $app -NetCoreVersion $version -OutputFolder $OutputFolder -BuildConfiguration $buildConfig -SkipBuild $SkipBuild -BuildR2R $r2r -BuildSelfContained $selfContained
                    }
                }
            }
        }
    }
}
else
{
    Invoke-CoreDumpGeneration -NetCoreVersion $NetCoreVersion -TestApplication $TestApplication -OutputFolder $OutputFolder -BuildConfiguration $BuildConfiguration -BuildR2R $BuildR2R -BuildSelfContained $BuildSelfContained -SkipBuild $SkipBuild
}