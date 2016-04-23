nuget pack .\FontBuddy.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget push *.nupkg