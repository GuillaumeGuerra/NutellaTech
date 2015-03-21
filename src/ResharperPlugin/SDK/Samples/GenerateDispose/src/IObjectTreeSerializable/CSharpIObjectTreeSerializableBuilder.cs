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
using System.Diagnostics;
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
using JetBrains.ReSharper.Psi.VB.Util;
using JetBrains.ReSharper.Refactorings.Move.Common;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
    [GeneratorBuilder("IObjectTreeSerializable", typeof(CSharpLanguage))]
    internal class CSharpIObjectTreeSerializableBuilder : GeneratorBuilderBase<CSharpGeneratorContext>
    {
        private readonly CodeAnnotationsCache _myCodeAnnotationsCache;

        public CSharpIObjectTreeSerializableBuilder(CodeAnnotationsCache codeAnnotationsCache)
        {
            _myCodeAnnotationsCache = codeAnnotationsCache;
        }

        public override double Priority
        {
            get { return 0; }
        }

        protected override void Process(CSharpGeneratorContext currentClassContext)
        {
            // TODO : implement the interface if the base class doesn't do it
            // TODO : find associated classes which have to be made serizalizable as well
            // TODO : try to provide a template for the newly generated fields
            // DONE : create the default constructor in the partial class if it doesn't exist
            // DONE : find the partial class if it already exists, instead of creating it
            // DONE : make sure to create the class in the same directory than the real class

            var factory = CSharpElementFactory.GetInstance(currentClassContext.Root.GetPsiModule());

            // Create the serialization partial class, or retrieve it if it does already exist
            bool partialClassCreated = false;
            var partialClassContext = GetPartialClassContext(currentClassContext, factory, out partialClassCreated);

            // Make sure the current class is partial
            currentClassContext.ClassDeclaration.SetPartial(true);

            var typeOwners = currentClassContext.InputElements.OfType<GeneratorDeclaredElement<ITypeOwner>>().ToList();

            CreateDefaultConstructor(currentClassContext, partialClassContext, factory, typeOwners);
            CreateObjectTreeConstructor(currentClassContext, partialClassContext, factory, typeOwners);
            CreateFillObjectTree(currentClassContext, partialClassContext, factory, typeOwners);
            CreateUpgradeObjectTree(currentClassContext, partialClassContext, factory, typeOwners);

            if (partialClassCreated)
            {
                WriteClassFile(currentClassContext, partialClassContext);
            }
        }

        #region Default Constructor

        private void CreateDefaultConstructor(CSharpGeneratorContext currentClassContext,
            CSharpGeneratorContext partialClassContext, CSharpElementFactory factory,
            List<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
        {
            var classType = currentClassContext.ClassDeclaration.DeclaredElement as IClass;
            if (classType != null && !classType.Constructors.Any(constructor => constructor.IsDefault && !constructor.IsPredefined))
            {
                var constructorDeclaration = factory.CreateConstructorDeclaration();
                partialClassContext.PutMemberDeclaration(constructorDeclaration, null,
                    newDeclaration => new GeneratorDeclarationElement(newDeclaration));
            }
        }

        #endregion

        #region ObjectTree Constructor

        private static void CreateObjectTreeConstructor(CSharpGeneratorContext currentClassContext, CSharpGeneratorContext partialClassContext, CSharpElementFactory factory, ICollection<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
        {
            var existingConstructor = FindObjectTreeConstructor(partialClassContext);
            if (existingConstructor != null)
            {
                var declaration = (IConstructorDeclaration)existingConstructor.GetDeclarations().FirstOrDefault();
                GenerateFillObjectTreeBody(declaration, typeOwners, factory);
            }
            else
            {
                var declaration = factory.CreateConstructorDeclaration();

                var parameter = factory.CreateParameterDeclaration(ParameterKind.UNKNOWN, false, false,
                    XOneTypesHelper.GetIObjectTreeInterface(partialClassContext), "tree", null);
                declaration.AddParameterDeclarationAfter(parameter, null);
                GenerateObjectTreeConstructorBody(declaration, typeOwners, factory);
                partialClassContext.PutMemberDeclaration(declaration, null,
                    newDeclaration => new GeneratorDeclarationElement(newDeclaration));
            }
        }

        private static void GenerateObjectTreeConstructorBody(ICSharpFunctionDeclaration methodDeclaration, IEnumerable<GeneratorDeclaredElement<ITypeOwner>> elements,
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

        private static IConstructor FindObjectTreeConstructor(CSharpGeneratorContext partialClassContext)
        {
            if (partialClassContext.ClassDeclaration.DeclaredElement == null)
                return null;

            return partialClassContext.ClassDeclaration.DeclaredElement.Constructors
                .FirstOrDefault(constructor =>
                {
                    if (constructor.Parameters.Count != 1)
                        return false;

                    var parameterTypeName = constructor.Parameters[0].Type.GetPresentableName(CSharpLanguage.Instance);
                    var iObjectTreeTypeName = XOneTypesHelper.GetIObjectTreeInterface(partialClassContext).GetPresentableName(CSharpLanguage.Instance);
                    return parameterTypeName == iObjectTreeTypeName;
                });
        }

        #endregion

        #region UpgradeObjectTree

        private void CreateUpgradeObjectTree(CSharpGeneratorContext currentClassContext, CSharpGeneratorContext partialClassContext, CSharpElementFactory factory, List<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
        {
            var existingMethod = FindUpgradeObjectTree(partialClassContext);
            if (existingMethod != null)
            {
                return;
            }
            var declaration = (IMethodDeclaration)factory.CreateTypeMemberDeclaration(
                "public void UpgradeObjectTree(IObjectTree tree);");
            GenerateUpgradeObjectTreeBody(partialClassContext, declaration, typeOwners, factory);
            partialClassContext.PutMemberDeclaration(declaration, null,
                newDeclaration => new GeneratorDeclarationElement(newDeclaration));
        }

        private static void GenerateUpgradeObjectTreeBody(CSharpGeneratorContext currentClassContext,
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

        private static void CreateFillObjectTree(CSharpGeneratorContext currentClassContext, CSharpGeneratorContext partialClassContext, CSharpElementFactory factory, ICollection<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
        {
            var existingMethod = FindFillObjectTree(partialClassContext);
            if (existingMethod != null)
            {
                var declaration = (IMethodDeclaration)existingMethod.GetDeclarations().FirstOrDefault();
                GenerateFillObjectTreeBody(declaration, typeOwners, factory);
            }
            else
            {
                var declaration = (IMethodDeclaration)factory.CreateTypeMemberDeclaration(
                    "public void FillObjectTree(IObjectTree tree);");
                GenerateFillObjectTreeBody(declaration, typeOwners, factory);
                partialClassContext.PutMemberDeclaration(declaration, null,
                    newDeclaration => new GeneratorDeclarationElement(newDeclaration));
            }
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

        #region Partial Class Management

        private CSharpGeneratorContext GetPartialClassContext(CSharpGeneratorContext currentClassContext,
            CSharpElementFactory factory, out bool partialClassCreated)
        {
            if (currentClassContext.ClassDeclaration.IsPartial)
            {
                IClass currentClass = currentClassContext.ClassDeclaration.DeclaredElement as IClass;
                var partialDeclarations = currentClass.GetDeclarations();
                var match = partialDeclarations.FirstOrDefault(declaration => declaration.GetSourceFile().Name == currentClass.ShortName + ".Serialization.cs");
                if (match != null && match is IClassDeclaration)
                {
                    partialClassCreated = false;
                    return CSharpGeneratorContext.CreateContext(currentClassContext.Kind, match as IClassLikeDeclaration, currentClassContext.Anchor);
                }
            }

            partialClassCreated = true;

            var declarationMergingBuilder =
                new TypeDeclarationMergingBuilder(currentClassContext.ClassDeclaration.DeclaredElement);
            declarationMergingBuilder.AddClassLikeDeclaration(currentClassContext.ClassDeclaration);

            IClassLikeDeclaration newDeclaration = declarationMergingBuilder.CreateClassLikeDeclaration(factory);

            var partialClassContext = CSharpGeneratorContext.CreateContext(currentClassContext.Kind, newDeclaration,
                currentClassContext.Anchor);

            // TODO : make this implementation only if the base class doesn't yet do it
            partialClassContext.ClassDeclaration.AddSuperInterface(
                TypeFactory.CreateType(XOneTypesHelper.GetIObjectTreeSerializableInterface(partialClassContext)), false);

            return partialClassContext;
        }

        private void WriteClassFile(CSharpGeneratorContext currentClassContext, CSharpGeneratorContext partialClassContext)
        {
            var file = TreeNodeExtensions.GetContainingFile(currentClassContext.ClassDeclaration);

            PsiExtensions.GetPsiServices(file.GetSolution()).Files.CommitAllDocuments();

            var partialClassFileName = currentClassContext.ClassDeclaration.DeclaredElement.ShortName + ".Serialization.cs";
            var partialClassFolder = currentClassContext.ClassDeclaration.GetProject().ProjectFile.ParentFolder.GetOrCreateProjectFolder(file.GetSourceFile().GetLocation().Directory);
            var partialClassContent = partialClassContext.ClassDeclaration.GetContainingFile().GetText();

            AddNewItemUtil.AddFile(partialClassFolder, partialClassFileName, partialClassContent);

            PsiExtensions.GetPsiServices(file.GetSolution()).Files.CommitAllDocuments();
        }

        #endregion
    }
}