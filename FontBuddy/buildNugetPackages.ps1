nuget pack .\FontBuddy.Android\FontBuddy.Android.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget pack .\FontBuddy.DesktopGL\FontBuddy.DesktopGL.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget pack .\FontBuddy.iOS\FontBuddy.iOS.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget pack .\FontBuddy.WindowsUniversal\FontBuddy.WindowsUniversal.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget push *.nupkg