rm *.nupkg
nuget pack .\FontBuddy.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget pack .\FontBuddy.Bridge.nuspec -IncludeReferencedProjects -Prop Configuration=Release
cp *.nupkg C:\Projects\Nugets\
nuget push *.nupkg -Source https://www.nuget.org/api/v2/package