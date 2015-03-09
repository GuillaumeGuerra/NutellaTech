using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Intentions.CSharp.ContextActions.Util;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.Util.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose.IObjectTreeSerializable
{
    internal class MergeClassDeclarationsItem : BulbActionBase
    {
        private readonly IClassLikeDeclaration myCurrentDeclaration;
        private readonly CSharpElementFactory myFactory;
        private IEnumerable<IProjectFile> myToRemove;
        private ICSharpFile myMainExchange;
        private ICSharpFile myMainFile;

        public override string Text
        {
            get
            {
                return "Merge partial declarations";
            }
        }

        public MergeClassDeclarationsItem(IClassLikeDeclaration currentDeclaration)
        {
            this.myCurrentDeclaration = currentDeclaration;
            this.myFactory = CSharpElementFactory.GetInstance((ITreeNode)this.myCurrentDeclaration, true);
            this.myCurrentDeclaration.GetPsiModule().GetSolution();
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progressIndicator)
        {
            this.myMainExchange = (ICSharpFile)null;
            ITypeElement declaredElement = this.myCurrentDeclaration.DeclaredElement;
            if (declaredElement == null)
            {
                Logger.Fail("typeElement == null");
                return (Action<ITextControl>)null;
            }
            TypeDeclarationMergingBuilder declarationMergingBuilder = new TypeDeclarationMergingBuilder(declaredElement);
            declarationMergingBuilder.AddClassLikeDeclaration(this.myCurrentDeclaration);
            bool flag = FileSpecificUtil.HasHiddenDeclarations(declaredElement);
            IList<ITypeDeclaration> relatedAndFilterHidden = FileSpecificUtil.GetRelatedAndFilterHidden(Enumerable.OfType<ITypeDeclaration>((IEnumerable)declaredElement.GetDeclarations()));
            foreach (IClassLikeDeclaration declaration in Enumerable.Except<IClassLikeDeclaration>(Enumerable.OfType<IClassLikeDeclaration>((IEnumerable)relatedAndFilterHidden), (IEnumerable<IClassLikeDeclaration>)new IClassLikeDeclaration[1]
      {
        this.myCurrentDeclaration
      }))
                declarationMergingBuilder.AddClassLikeDeclaration(declaration);
            IClassLikeDeclaration newDeclaration = declarationMergingBuilder.CreateClassLikeDeclaration(this.myFactory);
            newDeclaration = MergeClassDeclarationsItem.AddNonPartialDeclarationBefore((IProperTypeDeclaration)this.myCurrentDeclaration, newDeclaration);
            newDeclaration.SetPartial(true);
            this.myMainFile = TreeNodeExtensions.GetContainingFile((ITreeNode)newDeclaration) as ICSharpFile;
            List<ICSharpFile> list = new List<ICSharpFile>();
            foreach (ITypeDeclaration typeDeclaration in (IEnumerable<ITypeDeclaration>)relatedAndFilterHidden)
            {
                IClassLikeDeclaration classLikeDeclaration = typeDeclaration as IClassLikeDeclaration;
                if (classLikeDeclaration != null && classLikeDeclaration != newDeclaration)
                {
                    newDeclaration.AddDeclarationsRangeBefore(classLikeDeclaration.GetAllDeclarationsRange(), (ITreeNode)null);
                    ICSharpFile file = TreeNodeExtensions.GetContainingFile((ITreeNode)classLikeDeclaration) as ICSharpFile;
                    if (file == null)
                    {
                        Logger.Fail("file == null");
                    }
                    else
                    {
                        MergeClassDeclarationsItem.RemoveTypeDeclaration((IProperTypeDeclaration)classLikeDeclaration);
                        if (MergeClassDeclarationsItem.IsEmptyFile(file))
                            list.Add(file);
                    }
                }
            }
            foreach (ICSharpFile csharpFile in list)
            {
                if (PsiSourceFileExtensions.ToProjectFile(csharpFile.GetSourceFile()).GetDependentFiles().Contains(PsiSourceFileExtensions.ToProjectFile(this.myMainFile.GetSourceFile())))
                {
                    this.myMainExchange = csharpFile;
                    list.Remove(csharpFile);
                    list.Add(this.myMainFile);
                    break;
                }
            }
            newDeclaration.SetPartial(flag);
            this.MergePartialMethods(newDeclaration);
            int offset = TreeNodeExtensions.GetDocumentRange((ITreeNode)newDeclaration).TextRange.StartOffset;
            this.myToRemove = (IEnumerable<IProjectFile>)Enumerable.ToArray<IProjectFile>(Enumerable.Select<ICSharpFile, IProjectFile>((IEnumerable<ICSharpFile>)list, (Func<ICSharpFile, IProjectFile>)(file => PsiSourceFileExtensions.ToProjectFile(file.GetSourceFile()))));
            return (Action<ITextControl>)(textControl =>
            {
                ITextControl activateTextControl = TextControlUtils.GetAndActivateTextControl((ITreeNode)newDeclaration);
                if (activateTextControl == null)
                    return;
                ITextControlCaretEx.MoveTo(activateTextControl.Caret, offset, CaretVisualPlacement.Generic);
            });
        }

        protected override Action<ITextControl> ExecuteAfterPsiTransaction(ISolution solution, IProjectModelTransactionCookie cookie, IProgressIndicator progress)
        {
            if (this.myMainExchange != null)
            {
                IDocument document1 = this.myMainFile.GetSourceFile().Document;
                string text1 = document1.GetText();
                IDocument document2 = this.myMainExchange.GetSourceFile().Document;
                string text2 = document2.GetText();
                document1.ReplaceText(new TextRange(0, document1.GetTextLength()), text2);
                document2.ReplaceText(new TextRange(0, document2.GetTextLength()), text1);
            }
            foreach (IProjectFile projectFile in this.myToRemove)
            {
                if (Enumerable.All<IProjectFile>((IEnumerable<IProjectFile>)projectFile.GetDependentFiles(), file => Enumerable.Contains(this.myToRemove, file)))
                    cookie.Remove((IProjectItem)projectFile);
            }
            return (Action<ITextControl>)null;
        }

        private void MergePartialMethods(IClassLikeDeclaration typeDeclaration)
        {
            foreach (IGrouping<IMethod, IMethodDeclaration> grouping in Enumerable.GroupBy<IMethodDeclaration, IMethod>(typeDeclaration.MethodDeclarations.Where((Func<IMethodDeclaration, bool>)(meth => meth.IsPartial)), (Func<IMethodDeclaration, IMethod>)(meth => meth.DeclaredElement)))
            {
                List<IMethodDeclaration> list1 = Enumerable.ToList<IMethodDeclaration>(Enumerable.DefaultIfEmpty<IMethodDeclaration>(Enumerable.Where<IMethodDeclaration>((IEnumerable<IMethodDeclaration>)grouping, (Func<IMethodDeclaration, bool>)(declaration => declaration.Body != null)), Enumerable.First<IMethodDeclaration>((IEnumerable<IMethodDeclaration>)grouping)));
                List<IMethodDeclaration> list2 = Enumerable.ToList<IMethodDeclaration>(Enumerable.Except<IMethodDeclaration>((IEnumerable<IMethodDeclaration>)grouping, (IEnumerable<IMethodDeclaration>)list1));
                foreach (IMethodDeclaration methodDeclaration in list1)
                {
                    if (methodDeclaration.Body == null)
                        methodDeclaration.SetBody(this.myFactory.CreateEmptyBlock());
                }
                foreach (IMethodDeclaration destination in list1)
                {
                    destination.SetPartial(false);
                    MergeClassDeclarationsItem.CopyAttributes(destination, (IEnumerable<IMethodDeclaration>)list2);
                }
                foreach (IMethodDeclaration methodDeclaration in list2)
                    typeDeclaration.RemoveClassMemberDeclaration((IClassMemberDeclaration)methodDeclaration);
            }
        }

        private static void CopyAttributes(IMethodDeclaration destination, IEnumerable<IMethodDeclaration> sources)
        {
            foreach (IMethodDeclaration methodDeclaration in sources)
            {
                foreach (IAttribute attribute in methodDeclaration.Attributes)
                    destination.AddAttributeAfter(attribute, (IAttribute)null);
                for (int index = 0; index < destination.ParameterDeclarations.Count; ++index)
                {
                    IRegularParameterDeclaration parameterDeclaration1 = destination.ParameterDeclarations[index] as IRegularParameterDeclaration;
                    IRegularParameterDeclaration parameterDeclaration2 = methodDeclaration.ParameterDeclarations[index] as IRegularParameterDeclaration;
                    if (parameterDeclaration1 != null && parameterDeclaration2 != null)
                    {
                        foreach (IAttribute attribute in parameterDeclaration2.Attributes)
                            parameterDeclaration1.AddAttributeAfter(attribute, (IAttribute)null);
                    }
                }
            }
        }

        private static bool ParametersMatch(IMethodDeclaration destination, IMethodDeclaration source)
        {
            TreeNodeCollection<ICSharpParameterDeclaration> parameterDeclarations1 = destination.ParameterDeclarations;
            TreeNodeCollection<ICSharpParameterDeclaration> parameterDeclarations2 = source.ParameterDeclarations;
            if (parameterDeclarations1.Count != parameterDeclarations2.Count)
                return false;
            for (int index = 0; index < parameterDeclarations1.Count; ++index)
            {
                if (!object.Equals((object)parameterDeclarations1[index].Type, (object)parameterDeclarations2[index].Type))
                    return false;
            }
            return true;
        }

        private static IClassLikeDeclaration AddNonPartialDeclarationBefore(IProperTypeDeclaration currentDeclaration, IClassLikeDeclaration newDecl)
        {
            IClassLikeDeclaration classLikeDeclaration = ClassLikeDeclarationNavigator.GetByNestedTypeDeclaration((ICSharpTypeDeclaration)currentDeclaration) as IClassLikeDeclaration;
            if (classLikeDeclaration != null)
            {
                newDecl = (IClassLikeDeclaration)classLikeDeclaration.AddClassMemberDeclarationAfter<IClassMemberDeclaration>((IClassMemberDeclaration)newDecl, (IClassMemberDeclaration)currentDeclaration);
            }
            else
            {
                ICSharpNamespaceDeclaration byTypeDeclaration1 = CSharpNamespaceDeclarationNavigator.GetByTypeDeclaration((ICSharpTypeDeclaration)currentDeclaration);
                if (byTypeDeclaration1 != null)
                {
                    newDecl = (IClassLikeDeclaration)byTypeDeclaration1.AddTypeDeclarationAfter((ICSharpTypeDeclaration)newDecl, (ICSharpTypeDeclaration)currentDeclaration);
                }
                else
                {
                    ICSharpFile byTypeDeclaration2 = CSharpFileNavigator.GetByTypeDeclaration((ICSharpTypeDeclaration)currentDeclaration);
                    if (byTypeDeclaration2 != null)
                        newDecl = (IClassLikeDeclaration)byTypeDeclaration2.AddTypeDeclarationAfter((ICSharpTypeDeclaration)newDecl, (ICSharpTypeDeclaration)currentDeclaration);
                    else
                        Logger.Fail("Unable to Add declaration");
                }
            }
            return newDecl;
        }

        private static void RemoveTypeDeclaration(IProperTypeDeclaration classDecl)
        {
            IClassLikeDeclaration classLikeDeclaration = ClassLikeDeclarationNavigator.GetByNestedTypeDeclaration((ICSharpTypeDeclaration)classDecl) as IClassLikeDeclaration;
            if (classLikeDeclaration != null)
            {
                classLikeDeclaration.RemoveClassMemberDeclaration((IClassMemberDeclaration)classDecl);
            }
            else
            {
                ICSharpNamespaceDeclaration byTypeDeclaration1 = CSharpNamespaceDeclarationNavigator.GetByTypeDeclaration((ICSharpTypeDeclaration)classDecl);
                if (byTypeDeclaration1 != null)
                {
                    byTypeDeclaration1.RemoveTypeDeclaration((ICSharpTypeDeclaration)classDecl);
                }
                else
                {
                    ICSharpFile byTypeDeclaration2 = CSharpFileNavigator.GetByTypeDeclaration((ICSharpTypeDeclaration)classDecl);
                    if (byTypeDeclaration2 != null)
                        byTypeDeclaration2.RemoveTypeDeclaration((ICSharpTypeDeclaration)classDecl);
                    else
                        Logger.Fail("Unable to remove class");
                }
            }
        }

        private static bool IsEmptyFile(ICSharpFile file)
        {
            if (file.Attributes.Count == 0)
                return MergeClassDeclarationsItem.IsEmptyTypeAndNamespaceHolderDeclaration((ICSharpTypeAndNamespaceHolderDeclaration)file);
            return false;
        }

        private static bool IsEmptyTypeAndNamespaceHolderDeclaration(ICSharpTypeAndNamespaceHolderDeclaration ns)
        {
            INamespaceDeclaration namespaceDeclaration = ns as INamespaceDeclaration;
            if (namespaceDeclaration != null)
            {
                IDeclaredElement declaredElement = (IDeclaredElement)namespaceDeclaration.DeclaredElement;
                if (declaredElement != null && declaredElement.GetDeclarations().Count == 1)
                    return false;
            }
            if (ns.TypeDeclarations.Count != 0)
                return false;
            foreach (ICSharpTypeAndNamespaceHolderDeclaration ns1 in ns.NamespaceDeclarations)
            {
                if (!MergeClassDeclarationsItem.IsEmptyTypeAndNamespaceHolderDeclaration(ns1))
                    return false;
            }
            return true;
        }
    }
}
