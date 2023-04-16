Param(
    [Parameter(Position = 0, Mandatory)]
    [ValidateSet("DevInit", "Build", "Deploy")]
	[string] $Type
)

Switch ($Type)
{
    "DevInit" {
        pushd "$PSScriptRoot"
        gci .\Migrations\ -Exclude ".gitignore" | rm
        dotnet ef migrations add InitialMigration
        dotnet ef database update
        popd
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