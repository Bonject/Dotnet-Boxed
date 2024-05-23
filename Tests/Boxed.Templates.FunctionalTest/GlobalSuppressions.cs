// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "VSTHRD111:Use ConfigureAwait(bool)", Justification = "xUnit1030\tTest methods should not call ConfigureAwait(), as it may bypass parallelization limits. (https://xunit.net/xunit.analyzers/rules/xUnit1030", Scope = "member", Target = "~M:Boxed.Templates.FunctionalTest.ApiTemplateTest.RestoreBuildTest_ApiDefaults_SuccessfulAsync(System.String,System.String[])~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Usage", "VSTHRD111:Use ConfigureAwait(bool)", Justification = "xUnit1030\tTest methods should not call ConfigureAwait(), as it may bypass parallelization limits. (https://xunit.net/xunit.analyzers/rules/xUnit1030", Scope = "member", Target = "~M:Boxed.Templates.FunctionalTest.NuGetTemplateTest.RestoreBuildTest_NuGetDefaults_SuccessfulAsync(System.String,System.String[])~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Usage", "VSTHRD111:Use ConfigureAwait(bool)", Justification = "xUnit1030\tTest methods should not call ConfigureAwait(), as it may bypass parallelization limits. (https://xunit.net/xunit.analyzers/rules/xUnit1030", Scope = "module")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "Inconsistent rules which require spacing and come do not.", Scope = "member", Target = "~F:Boxed.Templates.FunctionalTest.NuGetTemplateTest.Arguments")]
