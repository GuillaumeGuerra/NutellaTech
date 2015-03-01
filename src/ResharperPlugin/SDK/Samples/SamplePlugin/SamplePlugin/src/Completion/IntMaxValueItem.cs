using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Psi.Transactions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace JetBrains.ReSharper.SamplePlugin.Completion
{
  public class IntMaxValueItem : TextLookupItemBase
  {
    private string name;
    private IconId image;

    public IntMaxValueItem(string name, PsiIconManager psiIconManager)
    {
      this.name = name;
      image = psiIconManager.GetImage(CLRDeclaredElementType.LOCAL_CONSTANT);
    }

    public override IconId Image
    {
      get { return image; }
    }

    public override string Text
    {
      get { return name; }
      set { name = value; }
    }

    protected override RichText GetDisplayName()
    {
      var displayName = new RichText(name);
      LookupUtil.AddEmphasize(displayName, new TextRange(0, displayName.Length));
      return displayName;
    }

    public override void Accept(ITextControl textControl, TextRange nameRange, LookupItemInsertType lookupItemInsertType, Suffix suffix,
      ISolution solution, bool keepCaretStill)
    {
      IIdentifier identifierNode = TextControlToPsi.GetElement<IIdentifier>(solution, textControl);
      var intMaxValue = CSharpElementFactory.GetInstance(identifierNode).CreateExpressionAsIs("int.MaxValue");
      IPsiServices psiServices = solution.GetPsiServices();
      if (identifierNode != null)
        using (var cookie = new PsiTransactionCookie(psiServices, DefaultAction.Rollback, "RemoveIdentifier"))
        using (new DisableCodeFormatter())
        {
          using (WriteLockCookie.Create())
            ModificationUtil.ReplaceChild(identifierNode, intMaxValue);

          cookie.Commit();
        }

      psiServices.Files.CommitAllDocuments();
    }
  }
}