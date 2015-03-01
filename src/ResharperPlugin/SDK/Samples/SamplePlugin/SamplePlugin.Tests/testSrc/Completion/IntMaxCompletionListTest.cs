using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using NUnit.Framework;

namespace JetBrains.ReSharper.SamplePlugin.Tests.Completion
{
  [TestFixture]
  public class IntMaxCompletionListTest : CodeCompletionTestBase
  {
    protected override string RelativeTestDataPath {  get { return @"CodeCompletion"; } }

    protected override bool ExecuteAction { get { return false; } }

    [Test] public void TestList01() { DoNamedTest(); }
    [Test] public void TestList02() { DoNamedTest(); }
  }
}