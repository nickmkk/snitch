$apikey = Read-Host -Prompt 'Input the nuget.org api-key';
dotnet pack src\Snitch\Snitch.csproj --configuration Release
dotnet nuget push .\src\Snitch\bin\release\*.nupkg  --source https://api.nuget.org/v3/index.json --api-key $apikey