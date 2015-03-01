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

using System.Linq;
using JetBrains.ReSharper.Feature.Services.CSharp.Generate;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
    /// <summary>
    /// Provides input elements for user's choice
    /// </summary>
    [GeneratorElementProvider("IObjectTreeSerializable", typeof(CSharpLanguage))]
    internal class CSharpIObjectTreeSerializableFieldProvider : GeneratorProviderBase<CSharpGeneratorContext>
    {
        #region IGeneratorElementProvider Members

        /// <summary>
        /// If we have several providers for the same generate kind, this property will set order on them
        /// </summary>
        public override double Priority
        {
            get { return 0; }
        }

        /// <summary>
        /// Populate context with input elements
        /// </summary>
        /// <param name="context"></param>
        public override void Populate(CSharpGeneratorContext context)
        {
            var typeElement = context.ClassDeclaration.DeclaredElement;
            if (typeElement == null)
                return;

            if (!(typeElement is IStruct) && !(typeElement is IClass))
                return;
            //var disposableType = TypeFactory.CreateType(GetDisposableInterface(context));

            context.ProvidedElements.AddRange(from member in typeElement.GetMembers().OfType<IField>()
                                              let memberType = member.Type as IDeclaredType
                                              where !member.IsStatic
                                                    && !member.IsConstant && !member.IsSynthetic()
                                                    && memberType != null
                                                    && memberType.CanUseExplicitly(context.ClassDeclaration)
                                              select new GeneratorDeclaredElement<ITypeOwner>(member));
            context.ProvidedElements.AddRange(from member in typeElement.GetMembers().OfType<IProperty>()
                                              let memberType = member.Type as IDeclaredType
                                              where !member.IsStatic
                                                    && !member.IsSynthetic()
                                                    && memberType != null
                                                    && memberType.CanUseExplicitly(context.ClassDeclaration)
                                              select new GeneratorDeclaredElement<ITypeOwner>(member));

        }

        #endregion
    }
}