dotnet restore src\AutoMapper.SelfConfig\project.json
dotnet pack src\AutoMapper.SelfConfig\project.json -c Release -o artifacts\bin\AutoMapper.SelfConfig\Release

set /p version="Version: "
echo artifacts\bin\AutoMapper.SelfConfig\Release\AutoMapper.SelfConfig.%version%.nupkg
nuget push artifacts\bin\AutoMapper.SelfConfig\Release\AutoMapper.SelfConfig.%version%.nupkg