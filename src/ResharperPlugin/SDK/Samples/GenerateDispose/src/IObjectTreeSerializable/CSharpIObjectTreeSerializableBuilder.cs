/*
 * Copyright 2007-2014 JetBrains
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Generate;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Feature.Services.Intentions.CreateDeclaration;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.DeclarationBuilders;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.PowerToys.GenerateDispose.IObjectTreeSerializable;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.Refactorings.Move.Common;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
    [GeneratorBuilder("IObjectTreeSerializable", typeof(CSharpLanguage))]
    internal class CSharpIObjectTreeSerializableBuilder : GeneratorBuilderBase<CSharpGeneratorContext>
    {
        private readonly CodeAnnotationsCache myCodeAnnotationsCache;

        public CSharpIObjectTreeSerializableBuilder(CodeAnnotationsCache codeAnnotationsCache)
        {
            myCodeAnnotationsCache = codeAnnotationsCache;
        }

        public override double Priority
        {
            get { return 0; }
        }

        protected override void Process(CSharpGeneratorContext context)
        {
            if (context.ClassDeclaration == null)
                return;

            var projectFile = context.ClassDeclaration.GetProject().ProjectFile;

            var declaredElement = context.ClassDeclaration.DeclaredElement;
            context.ClassDeclaration.SetPartial(true);

            var factory = CSharpElementFactory.GetInstance(context.Root.GetPsiModule());
            var typeOwners = context.InputElements.OfType<GeneratorDeclaredElement<ITypeOwner>>().ToList();

            var declarationMergingBuilder = new TypeDeclarationMergingBuilder(declaredElement);
            declarationMergingBuilder.AddClassLikeDeclaration(context.ClassDeclaration);

            IClassLikeDeclaration newDeclaration = declarationMergingBuilder.CreateClassLikeDeclaration(factory);

            var currentContext = CSharpGeneratorContext.CreateContext(context.Kind, newDeclaration, context.Anchor);
            // order is important
            CreateConstructor(context, currentContext, factory, typeOwners);
            CreateFillObjectTree(context, currentContext, factory, typeOwners);
            CreateUpgradeObjectTree(context, currentContext, factory, typeOwners);
            if (context.GetGlobalOptionValue("ImplementIObjectTreeSerializable") == bool.TrueString)
            {
                var type = GetIObjectTreeSerializableInterface(currentContext);
                if (type != null)
                {
                    var ownTypeElement = declaredElement;
                    if (ownTypeElement != null)
                        currentContext.ClassDeclaration.AddSuperInterface(TypeFactory.CreateType(type), false);
                }
            }

            var file = TreeNodeExtensions.GetContainingFile((ITreeNode)context.ClassDeclaration) as ICSharpFile;
            PsiExtensions.GetPsiServices(file.GetSolution()).Files.CommitAllDocuments();
            //var projectFile = PsiSourceFileExtensions.ToProjectFile(file.GetSourceFile());
            var cSharpFile = AddNewItemUtil.AddFile(projectFile.ParentFolder, context.ClassDeclaration.DeclaredElement.ShortName + ".Serialization.cs", TreeNodeExtensions.GetContainingFile((ITreeNode)newDeclaration).GetText()).GetPrimaryPsiFile() as ICSharpFile;

            PsiExtensions.GetPsiServices(file.GetSolution()).Files.CommitAllDocuments();
        }

        #region UpgradeObjectTree

        private void CreateUpgradeObjectTree(CSharpGeneratorContext context, CSharpGeneratorContext currentContext, CSharpElementFactory factory, List<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
        {
            var existingMethod = FindUpgradeObjectTree(currentContext);
            if (existingMethod != null)
            {
                return;
            }
            var declaration = (IMethodDeclaration)factory.CreateTypeMemberDeclaration(
                "public void UpgradeObjectTree(IObjectTree tree);");
            GenerateUpgradeObjectTreeBody(currentContext, declaration, typeOwners, factory);
            currentContext.PutMemberDeclaration(declaration, null,
                newDeclaration => new GeneratorDeclarationElement(newDeclaration));
        }

        private static void GenerateUpgradeObjectTreeBody(CSharpGeneratorContext context,
            ICSharpFunctionDeclaration methodDeclaration, ICollection<GeneratorDeclaredElement<ITypeOwner>> elements,
            CSharpElementFactory factory)
        {
            var builder = new StringBuilder();
            var owner = (IParametersOwner)methodDeclaration.DeclaredElement;
            if (owner == null)
                return;

            builder.AppendLine("// TODO : write upgrade code");
            methodDeclaration.SetBody(factory.CreateBlock("{" + builder + "}"));
        }

        private static IOverridableMember FindUpgradeObjectTree(CSharpGeneratorContext context)
        {
            if (context.ClassDeclaration.DeclaredElement == null)
                return null;

            return context.ClassDeclaration.DeclaredElement.Methods
                .FirstOrDefault(method => method.ShortName == "UpgradeObjectTree"
                                          && method.ReturnType.IsVoid()
                                          && method.Parameters.Count == 1);
        }

        #endregion

        #region FillObjectTree

        private static void CreateFillObjectTree(CSharpGeneratorContext context, CSharpGeneratorContext currentContext, CSharpElementFactory factory, ICollection<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
        {
            var existingMethod = FindFillObjectTree(currentContext);
            IMethodDeclaration declaration;
            if (existingMethod != null)
            {
                if (context.GetGlobalOptionValue("ChangeFillObjectTree") == "Skip")
                    return;
                if (context.GetGlobalOptionValue("ChangeFillObjectTree") == "Replace")
                {
                    declaration = (IMethodDeclaration)existingMethod.GetDeclarations().FirstOrDefault();
                    GenerateFillObjectTreeBody(declaration, typeOwners, factory);
                    return;
                }
            }
            declaration = (IMethodDeclaration)factory.CreateTypeMemberDeclaration(
                "public void FillObjectTree(IObjectTree tree);");
            GenerateFillObjectTreeBody(declaration, typeOwners, factory);
            currentContext.PutMemberDeclaration(declaration, null,
                newDeclaration => new GeneratorDeclarationElement(newDeclaration));
        }

        private static void GenerateFillObjectTreeBody(ICSharpFunctionDeclaration methodDeclaration, ICollection<GeneratorDeclaredElement<ITypeOwner>> elements,
            CSharpElementFactory factory)
        {
            var builder = new StringBuilder();
            var owner = (IParametersOwner)methodDeclaration.DeclaredElement;
            if (owner == null)
                return;

            foreach (var element in elements)
            {
                var typeOwner = element.DeclaredElement;
                var type = typeOwner.Type;

                builder.Append(string.Format("tree.Set<{1}>(\"{0}\",{0});", element.DeclaredElement.ShortName, type.GetPresentableName(CSharpLanguage.Instance)));
            }
            methodDeclaration.SetBody(factory.CreateBlock("{" + builder + "}"));
        }

        private static IOverridableMember FindFillObjectTree(CSharpGeneratorContext context)
        {
            if (context.ClassDeclaration.DeclaredElement == null)
                return null;

            return context.ClassDeclaration.DeclaredElement.Methods
                .FirstOrDefault(method => method.ShortName == "FillObjectTree"
                                          && method.ReturnType.IsVoid()
                                          && method.Parameters.Count == 1);
        }

        #endregion

        #region ObjectTree Constructor

        private static void CreateConstructor(CSharpGeneratorContext context, CSharpGeneratorContext currentContext, CSharpElementFactory factory, ICollection<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
        {
            var existingConstructor = FindConstructor(currentContext.ClassDeclaration);
            IConstructorDeclaration declaration;
            if (existingConstructor != null)
            {
                if (context.GetGlobalOptionValue("ChangeFillObjectTree") == "Skip")
                    return;
                if (context.GetGlobalOptionValue("ChangeFillObjectTree") == "Replace")
                {
                    declaration = (IConstructorDeclaration)existingConstructor.GetDeclarations().FirstOrDefault();
                    GenerateFillObjectTreeBody(declaration, typeOwners, factory);
                    return;
                }
            }
            var constructorDeclaration = factory.CreateConstructorDeclaration();

            var iObjectTreeInterface = GetIObjectTreeInterface(currentContext);
            var parameter = factory.CreateParameterDeclaration(ParameterKind.UNKNOWN, false, false, iObjectTreeInterface, "tree", null);
            constructorDeclaration.AddParameterDeclarationAfter(parameter, null);
            declaration = (IConstructorDeclaration)constructorDeclaration.DeclaredElement.GetDeclarations().FirstOrDefault();
            GenerateConstructorBody(currentContext, declaration, typeOwners, factory);
            currentContext.PutMemberDeclaration(declaration, null,
                newDeclaration => new GeneratorDeclarationElement(newDeclaration));
        }

        private static void GenerateConstructorBody(CSharpGeneratorContext context,
            ICSharpFunctionDeclaration methodDeclaration, ICollection<GeneratorDeclaredElement<ITypeOwner>> elements,
            CSharpElementFactory factory)
        {
            var builder = new StringBuilder();
            var owner = (IParametersOwner)methodDeclaration.DeclaredElement;
            if (owner == null)
                return;

            foreach (var element in elements)
            {
                var typeOwner = element.DeclaredElement;
                var type = typeOwner.Type;

                builder.Append(string.Format("{0}=tree.Get<{1}>(\"{0}\");", element.DeclaredElement.ShortName, type.GetPresentableName(CSharpLanguage.Instance)));
            }
            methodDeclaration.SetBody(factory.CreateBlock("{" + builder + "}"));
        }

        private static IConstructor FindConstructor(IClassLikeDeclaration classDeclaration)
        {
            if (classDeclaration.DeclaredElement == null)
                return null;

            return classDeclaration.DeclaredElement.Constructors
                .FirstOrDefault(constructor => constructor.Parameters.Count == 1);
        }

        #endregion

        protected override IList<IGeneratorOption> GetGlobalOptions(CSharpGeneratorContext context)
        {
            var hasReferenceFields = context.ProvidedElements
              .OfType<GeneratorDeclaredElement<ITypeOwner>>()
              .Any(field => field.DeclaredElement.Type.IsReferenceType());

            var options = new List<IGeneratorOption>();
            if (!HasIObjectTreeSerializable(context))
                options.Add(new GeneratorOptionBoolean("ImplementIObjectTreeSerializable", "Implement IObjectTreeSerializable interface", true) { Persist = true });
            if (FindFillObjectTree(context) != null)
                options.Add(new GeneratorOptionSelector("ChangeFillObjectTree", "FillObjectTree already exists", "Replace", new[] { "Replace", "Skip", "Side by side" }) { Persist = true });

            return options;
        }

        private static bool HasIObjectTreeSerializable(CSharpGeneratorContext context)
        {
            var type = GetIObjectTreeSerializableInterface(context);
            if (type == null)
                return false;
            var ownTypeElement = context.ClassDeclaration.DeclaredElement;
            if (ownTypeElement == null)
                return false;
            var ownType = TypeFactory.CreateType(ownTypeElement);
            var disposableType = TypeFactory.CreateType(type);
            return ownType.IsSubtypeOf(disposableType);
        }

        private static ITypeElement GetIObjectTreeSerializableInterface(IGeneratorContext context)
        {
            //TODO : change namespace
            return TypeFactory.CreateTypeByCLRName("ResharperPluginTestProject.IObjectTreeSerializable", context.PsiModule, context.Anchor.GetResolveContext()).GetTypeElement();
        }

        private static IType GetIObjectTreeInterface(IGeneratorContext context)
        {
            //TODO : change namespace
            return TypeFactory.CreateTypeByCLRName("ResharperPluginTestProject.IObjectTree", context.PsiModule, context.Anchor.GetResolveContext()).ToIType();
        }

        protected override IList<IGeneratorOption> GetInputElementOptions(IGeneratorElement inputElement,
                                                                          CSharpGeneratorContext context)
        {
            var declaredElement = inputElement as GeneratorDeclaredElement<ITypeOwner>;
            if (declaredElement != null)
            {
                var typeOwner = declaredElement.DeclaredElement;
                if (typeOwner.Type.IsReferenceType())
                {
                    var attributesOwner = typeOwner as IAttributesOwner;
                    var mark = myCodeAnnotationsCache.GetNullableAttribute(attributesOwner);
                    return new IGeneratorOption[]
          {
            new GeneratorOptionBoolean(CSharpBuilderOptions.CanBeNull, "Can be &null",
                                       mark != CodeAnnotationNullableValue.NOT_NULL)
              { OverridesGlobalOption = mark == CodeAnnotationNullableValue.NOT_NULL }
          };
                }
            }
            return base.GetInputElementOptions(inputElement, context);
        }

        protected override bool HasProcessableElements(CSharpGeneratorContext context, IEnumerable<IGeneratorElement> elements)
        {
            return true;
        }
    }
}