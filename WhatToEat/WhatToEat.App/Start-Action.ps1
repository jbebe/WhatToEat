Param(
    [Parameter(Position = 0, Mandatory)]
    [ValidateSet("Build","Deploy")]
	[string] $Type
)

Switch ($Type)
{
    "DevInit" {
        dotnet ef migrations add InitialMigration
        dotnet ef database update
    }
    "Build" {
        pushd "$(PSScriptRoot)\.."
        docker image rm bjuhasz/whattoeat:latest | Out-Null
        docker build -t bjuhasz/whattoeat:latest -f .\WhatToEat.App\Dockerfile .    
        popd
    }
    "Deploy" {
        docker push bjuhasz/whattoeat:latest
    }
}