namespace Boxed.Templates.FunctionalTest;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Boxed.DotnetNewTest;
using Xunit;
using Xunit.Abstractions;

[Trait("Template", "NuGet")]
public class NuGetTemplateTest
{
    private const string TemplateName = "nuget";
    private const string SolutionFileName = "NuGetTemplate.sln";
    private static readonly string[] Arguments = ["sign=false"];

    public NuGetTemplateTest(ITestOutputHelper testOutputHelper)
    {
        ArgumentNullException.ThrowIfNull(testOutputHelper);

        TestLogger.WriteMessage = testOutputHelper.WriteLine;
    }

    [Theory]
    [Trait("IsUsingDocker", "false")]
    [Trait("IsUsingDotnetRun", "false")]
    [InlineData("NuGetDefaults")]
    [InlineData("NuGetStyleCop", "style-cop=true")]
    [InlineData("NuGetDotnetCore", "framework=netstandard2.0")]
    [InlineData("NuGetDotnetFramework", "framework=net472")]
    [InlineData("NuGetAnalysisAll", "analysis-mode=AllEnabledByDefault")]
    [InlineData("NuGetAnalysisNone", "analysis-mode=AllDisabledByDefault")]
    [InlineData("NuGetNoImplicitUsings", "implicit-usings=false")]
    [InlineData("NuGetTrimmable", "trimmable=true")]
    [InlineData("NuGetNoComments", "comments=false")]
    public async Task RestoreBuildTest_NuGetDefaults_SuccessfulAsync(string name, params string[] arguments)
    {
        await InstallTemplateAsync();
        await using (var tempDirectory = TempDirectory.NewTempDirectory())
        {
            var project = await tempDirectory
                .DotnetNewAsync(TemplateName, name, arguments.ToArguments())
;
            await project.DotnetRestoreWithRetryAsync();
            await project.DotnetBuildAsync();

            if (!arguments.Contains("framework=net472") ||
                (arguments.Contains("framework=net472") && Environment.OSVersion.Platform == PlatformID.Win32NT))
            {
                // There seems to be a bug that stops xUnit working on Mono.
                await project.DotnetTestAsync();
            }
        }
    }

    [Theory]
    [Trait("IsUsingDocker", "false")]
    [Trait("IsUsingDotnetRun", "false")]
    [InlineData("NuGetDefaults")]
    public async Task Cake_NuGetDefaults_SuccessfulAsync(string name, params string[] arguments)
    {
        await InstallTemplateAsync();
        await using (var tempDirectory = TempDirectory.NewTempDirectory())
        {
            var project = await tempDirectory
                .DotnetNewAsync(TemplateName, name, arguments.ToArguments())
;
            await project.DotnetToolRestoreAsync();
            await project.DotnetCakeAsync();
        }
    }

    [Theory]
    [Trait("IsUsingDocker", "false")]
    [Trait("IsUsingDotnetRun", "false")]
    [InlineData("NuGetNoAppVeyor", "appveyor.yml", "AppVeyor", "appveyor=false")]
    [InlineData("NuGetNoAzurePipelines", "azure-pipelines.yml", "AzurePipelines", "azure-pipelines=false")]
    [InlineData("NuGetNoGitHubActions", @"/.github/workflows/build.yml", "GitHubActions", "github-actions=false")]
    public async Task RestoreBuildTestCake_NoContinuousIntegration_SuccessfulAsync(
        string name,
        string relativeFilePath,
        string cakeContent,
        params string[] arguments)
    {
        await InstallTemplateAsync();
        await using (var tempDirectory = TempDirectory.NewTempDirectory())
        {
            var project = await tempDirectory
                .DotnetNewAsync(TemplateName, name, arguments.ToArguments())
;
            await project.DotnetRestoreWithRetryAsync();
            await project.DotnetBuildAsync();
            await project.DotnetTestAsync();
            await project.DotnetToolRestoreAsync();
            await project.DotnetCakeAsync();

            Assert.False(File.Exists(Path.Combine(project.DirectoryPath, relativeFilePath)));
            var cake = await File.ReadAllTextAsync(Path.Combine(project.DirectoryPath, "build.cake"));
            Assert.DoesNotContain(cakeContent, cake, StringComparison.Ordinal);
        }
    }

    [Fact]
    [Trait("IsUsingDocker", "false")]
    [Trait("IsUsingDotnetRun", "false")]
    public async Task RestoreBuildTest_SignFalse_SuccessfulAsync()
    {
        await InstallTemplateAsync();
        await using (var tempDirectory = TempDirectory.NewTempDirectory())
        {
            var project = await tempDirectory
                .DotnetNewAsync(
                    TemplateName,
                    "NuGetSignFalse",
                    Arguments.ToArguments())
;
            await project.DotnetRestoreWithRetryAsync();
            await project.DotnetBuildAsync();
            await project.DotnetTestAsync();

            var files = new DirectoryInfo(project.DirectoryPath).GetFiles("*.*", SearchOption.AllDirectories);

            var csprojFile = files.Single(x => x.Name == "NuGetSignFalse.csproj");
            var csproj = File.ReadAllText(csprojFile.FullName);
            Assert.DoesNotContain("Sign", csproj, StringComparison.Ordinal);
        }
    }

    private static Task InstallTemplateAsync() => DotnetNew.InstallAsync<NuGetTemplateTest>(SolutionFileName);
}
