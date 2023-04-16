Param(
    [Parameter(Position = 0, Mandatory)]
    [ValidateSet("Build","Deploy")]
	[string] $Type
)

Switch ($Type)
{
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