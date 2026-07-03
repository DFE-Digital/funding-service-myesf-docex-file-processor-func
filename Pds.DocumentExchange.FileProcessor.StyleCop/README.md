Instructions for adding StyleCop when adding new projects to the solution:

1. Add the StyleCop.Analyzers nuget package
1. Add stylecop.json from this Shared project as a linked file in the root of your new project
1. In the properties of the linked stylecop.json file, change the build action to "c# analyzer additional file"
1. Update the csproj file for your new project to ensure it contains the following:

```xml
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <CodeAnalysisRuleSet>..\Pds.DocumentExchange.FileProcessor.StyleCop\PDS.CodeAnalysis.ruleset</CodeAnalysisRuleSet>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <CodeAnalysisRuleSet>..\Pds.DocumentExchange.FileProcessor.StyleCop\PDS.CodeAnalysis.ruleset</CodeAnalysisRuleSet>
  </PropertyGroup>
```