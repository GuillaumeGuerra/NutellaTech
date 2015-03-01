using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using NUnit.Framework;

namespace JetBrains.ReSharper.SamplePlugin.Tests.Completion
{
  [TestFixture]
  public class IntMaxCompletionTest : CodeCompletionTestBase
  {
    protected override string RelativeTestDataPath {  get { return @"CodeCompletion"; } }

    protected override bool ExecuteAction { get { return true; } }

    [Test] public void Test01() { DoNamedTest(); }
  }
}