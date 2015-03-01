using JetBrains.ProjectModel;

namespace JetBrains.ReSharper.SamplePlugin.Completion
{
  using Feature.Services.CodeCompletion;
  using Feature.Services.CodeCompletion.Infrastructure;
  using Feature.Services.CSharp.CodeCompletion.Infrastructure;
  using Feature.Services.Lookup;
  using Psi;
  using Psi.CSharp;
  using Psi.Services;
  using Psi.Tree;

  /// <summary>
  /// Allows you to type <c>maxint</c> and turn it into <c>int.MaxValue</c>.
  /// </summary>
  [Language(typeof(CSharpLanguage))]
  public class IntMaxValueRule : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
  {
    protected override bool IsAvailable(CSharpCodeCompletionContext context)
    {
      var type = context.BasicContext.CodeCompletionType;
      if (type == CodeCompletionType.AutomaticCompletion ||
          type == CodeCompletionType.BasicCompletion) return !context.IsQualified;
      return false;
    }

    public override bool IsEvaluationModeSupported(CodeCompletionParameters parameters)
    {
      return parameters.EvaluationMode == EvaluationMode.Light;
    }

    protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
    {
      ITreeNode node = TextControlToPsi.GetElement<ITreeNode>(context.BasicContext.Solution, context.BasicContext.TextControl);
      if (node == null) return false;

      if (!(node is IIdentifier)) return false;

      var item = new IntMaxValueItem("maxint", context.BasicContext.Solution.GetComponent<PsiIconManager>());
      item.InitializeRanges(context.CompletionRanges, context.BasicContext);
      collector.AddAtDefaultPlace(item);

      return true;
    }
  }
}