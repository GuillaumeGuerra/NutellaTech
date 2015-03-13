using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose.IObjectTreeSerializable
{
    public class TypeDeclarationMergingBuilder
    {
        private readonly List<IAttribute> myAttributes = new List<IAttribute>();
        private readonly List<IType> myBaseClasses = new List<IType>();
        private readonly List<IType> myBaseInterfaces = new List<IType>();
        private readonly Dictionary<string, TypeDeclarationMergingBuilder.TypeParameterInformation> myTypeParameters = new Dictionary<string, TypeDeclarationMergingBuilder.TypeParameterInformation>();
        private readonly AccessRights myAccessRights;
        private readonly string myName;
        private StringBuilder myGeneratedCode;
        private List<object> myGeneratedParams;
        private bool myIsAbstract;
        private bool myIsNew;
        private bool myIsSealed;
        private bool myIsStatic;
        private bool myIsUnsafe;
        private string myStructureType;
        private List<IUsingDirective> myUsings = new List<IUsingDirective>();
        private INamespace myNamespace;

        public TypeDeclarationMergingBuilder(ITypeElement element)
        {
            this.myName = element.ShortName;
            this.myAccessRights = ((IAccessRightsOwner)element).GetAccessRights();
        }

        private TypeDeclarationMergingBuilder.TypeParameterInformation GetTypeParameter(string name)
        {
            if (this.myTypeParameters.ContainsKey(name))
                return this.myTypeParameters[name];
            TypeDeclarationMergingBuilder.TypeParameterInformation parameterInformation = new TypeDeclarationMergingBuilder.TypeParameterInformation();
            this.myTypeParameters[name] = parameterInformation;
            return parameterInformation;
        }

        private void AddTypeParameterDeclaration(ITypeParameterDeclaration element)
        {
            TypeDeclarationMergingBuilder.TypeParameterInformation typeParameter = this.GetTypeParameter(element.NameIdentifier.Name);
            typeParameter.Attributes = (IList<IAttribute>)CollectionUtil.Then<IAttribute>((IEnumerable<IAttribute>)typeParameter.Attributes, (IEnumerable<IAttribute>)element.Attributes);
        }

        private void AddTypeParameterConstraints(ITypeParameterConstraintsClause clause)
        {
            string name = clause.TypeParameter.NameIdentifier.Name;
            TypeDeclarationMergingBuilder.TypeParameterInformation typeParameter = this.GetTypeParameter(name);
            if (typeParameter.Constraint == null)
                typeParameter.Constraint = new TypeDeclarationMergingBuilder.TypeParameterConstraint(name);
            typeParameter.Constraint.AddTypeParameterConstraintClause((IEnumerable<ITypeParameterConstraint>)clause.Constraints);
        }

        private void AddSuperClassOrInterface(IDeclaredType type)
        {
            ITypeElement typeElement = type.GetTypeElement();
            if (typeElement is IClass || typeElement is IStruct)
                TypeDeclarationMergingBuilder.AddToList((IList<IType>)this.myBaseClasses, (IType)type);
            else
                TypeDeclarationMergingBuilder.AddToList((IList<IType>)this.myBaseInterfaces, (IType)type);
        }

        public void AddClassLikeDeclaration(IClassLikeDeclaration declaration)
        {
            foreach (ITypeParameterDeclaration element in declaration.TypeParameters)
                this.AddTypeParameterDeclaration(element);
            foreach (ITypeParameterConstraintsClause clause in declaration.TypeParameterConstraintsClauses)
                this.AddTypeParameterConstraints(clause);
            if (declaration.IsAbstract)
                this.myIsAbstract = true;
            if (declaration.IsSealed)
                this.myIsSealed = true;
            if (declaration.IsUnsafe)
                this.myIsUnsafe = true;
            IModifiersList modifiersList = declaration.ModifiersList;
            if (modifiersList != null)
            {
                if (modifiersList.HasModifier(CSharpTokenType.NEW_KEYWORD))
                    this.myIsNew = true;
                if (modifiersList.HasModifier(CSharpTokenType.STATIC_KEYWORD))
                    this.myIsStatic = true;
            }
            foreach (IDeclaredType type in declaration.SuperTypes)
                this.AddSuperClassOrInterface(type);
            if (this.myStructureType == null)
            {
                if (declaration is IClassDeclaration)
                    this.myStructureType = " class ";
                else if (declaration is IStructDeclaration)
                    this.myStructureType = " struct ";
                else if (declaration is IInterfaceDeclaration)
                    this.myStructureType = " interface ";
            }
            this.myAttributes.AddRange((IEnumerable<IAttribute>)declaration.Attributes);
            this.myUsings.AddRange((declaration.DeclaredElement.GetSourceFiles().First().GetTheOnlyPsiFile(CSharpLanguage.Instance) as ICSharpFile).Imports);
            this.myNamespace = declaration.DeclaredElement.GetContainingNamespace();
        }

        private void GenerateTypeParametersDeclaration()
        {
            if (this.myTypeParameters.Count == 0)
                return;
            this.myGeneratedCode.Append('<');
            bool isFirst = true;
            foreach (KeyValuePair<string, TypeDeclarationMergingBuilder.TypeParameterInformation> keyValuePair in this.myTypeParameters)
            {
                this.EnsureDelimiter(ref isFirst);
                if (keyValuePair.Value.Attributes.Count > 0)
                    this.GenerateAttributeUsages((IEnumerable<IAttribute>)keyValuePair.Value.Attributes);
                this.AppendParameter((object)keyValuePair.Key);
            }
            this.myGeneratedCode.Append('>');
        }

        private void GenerateTypeParameterConstraints()
        {
            bool flag = false;
            foreach (TypeDeclarationMergingBuilder.TypeParameterInformation parameterInformation in this.myTypeParameters.Values)
            {
                if (flag)
                    this.myGeneratedCode.Append(' ');
                TypeDeclarationMergingBuilder.TypeParameterConstraint parameterConstraint = parameterInformation.Constraint;
                if (parameterConstraint != null)
                    flag = parameterConstraint.GenerateTypeParameterConstraint(this);
            }
        }

        private void GenerateExtendsList()
        {
            if (this.myBaseClasses.Count == 0 && this.myBaseInterfaces.Count == 0)
                return;
            this.myGeneratedCode.Append(" : ");
            bool isFirst = true;
            foreach (IType type in this.myBaseClasses)
            {
                this.EnsureDelimiter(ref isFirst);
                this.AppendParameter((object)type);
            }
            foreach (IType type in this.myBaseInterfaces)
            {
                this.EnsureDelimiter(ref isFirst);
                this.AppendParameter((object)type);
            }
        }

        private void GenerateAttributeUsages(IEnumerable<IAttribute> attributes)
        {
            foreach (IAttribute attribute in attributes)
            {
                this.myGeneratedCode.Append('[');
                this.AppendParameter((object)attribute);
                this.myGeneratedCode.Append(']');
            }
        }

        private void CreateFactoryParams()
        {
            myGeneratedCode.Append("namespace ");
            this.AppendParameter(myNamespace.ShortName);
            myGeneratedCode.AppendLine(" { ");
            //this.GenerateAttributeUsages((IEnumerable<IAttribute>)this.myAttributes);
            this.myGeneratedCode.Append(this.myStructureType);
            this.AppendParameter((object)this.myName);
            this.GenerateTypeParametersDeclaration();
            this.GenerateExtendsList();
            this.GenerateTypeParameterConstraints();
        }

        public IClassLikeDeclaration CreateClassLikeDeclaration(CSharpElementFactory factory)
        {
            this.myGeneratedCode = new StringBuilder();
            this.myGeneratedParams = new List<object>();
            this.CreateFactoryParams();
            this.myGeneratedCode.Append("{} }");
            var file = factory.CreateFile(((object)this.myGeneratedCode).ToString(), this.myGeneratedParams.ToArray());
            myUsings.ForEach(u => file.AddImport(u));
            IClassLikeDeclaration typeDeclaration = file.NamespaceDeclarations[0].TypeDeclarations[0] as IClassLikeDeclaration;
            typeDeclaration.GetContainingNamespaceDeclaration().SetQualifiedName(myNamespace.ShortName);
            typeDeclaration.SetAbstract(this.myIsAbstract);
            typeDeclaration.SetUnsafe(this.myIsUnsafe);
            typeDeclaration.SetSealed(this.myIsSealed);
            this.SetStaticAndNew(factory, typeDeclaration);
            typeDeclaration.SetAccessRights(this.myAccessRights);
            typeDeclaration.SetPartial(true);
            return typeDeclaration;
        }

        private void SetStaticAndNew(CSharpElementFactory factory, IClassLikeDeclaration typeDeclaration)
        {
            if (!this.myIsNew && !this.myIsStatic)
                return;
            string format = "void Foo()";
            if (this.myIsNew)
                format = "new " + format;
            if (this.myIsStatic)
                format = "static " + format;
            IModifiersList modifiersList = ((IMethodDeclaration)factory.CreateTypeMemberDeclaration(format, new object[0])).ModifiersList;
            if (typeDeclaration.ModifiersList == null)
            {
                typeDeclaration.SetModifiersList(modifiersList);
            }
            else
            {
                foreach (ITokenNode modifierNode in modifiersList.Modifiers)
                    typeDeclaration.ModifiersList.AddModifier(modifierNode);
            }
        }

        private void EnsureDelimiter(ref bool isFirst)
        {
            if (!isFirst)
                this.myGeneratedCode.Append(", ");
            else
                isFirst = false;
        }

        private void AppendParameter(object parameter)
        {
            this.myGeneratedCode.Append('$').Append(this.myGeneratedParams.Count);
            this.myGeneratedParams.Add(parameter);
        }

        private static void AddToList(IList<IType> list, IType type)
        {
            if (list.IndexOf(type) >= 0)
                return;
            list.Add(type);
        }

        private class TypeParameterConstraint
        {
            private readonly List<IType> mySuperTypes = new List<IType>();
            private readonly string myName;
            private bool myHasConstructor;
            private bool myIsClass;
            private bool myIsStruct;

            public string Name
            {
                get
                {
                    return this.myName;
                }
            }

            public TypeParameterConstraint(string name)
            {
                this.myName = name;
            }

            public void AddTypeParameterConstraintClause(IEnumerable<ITypeParameterConstraint> constrs)
            {
                foreach (ITypeParameterConstraint parameterConstraint in constrs)
                {
                    if (parameterConstraint is ITypeConstraint)
                    {
                        IType constraintType = ((ITypeConstraint)parameterConstraint).Constraint;
                        if (Enumerable.All<IType>((IEnumerable<IType>)this.mySuperTypes, (Func<IType, bool>)(type => !constraintType.Equals((object)type))))
                            this.mySuperTypes.Add(constraintType);
                    }
                    else if (parameterConstraint is IConstructorConstraint)
                        this.myHasConstructor = true;
                    else if (parameterConstraint is IReferenceConstraint)
                        this.myIsClass = true;
                    else if (parameterConstraint is IValueConstraint)
                        this.myIsStruct = true;
                }
            }

            public bool GenerateTypeParameterConstraint(TypeDeclarationMergingBuilder mergingBuilder)
            {
                if (this.mySuperTypes.Count == 0 && !this.myIsClass && (!this.myIsStruct && !this.myHasConstructor))
                    return false;
                mergingBuilder.myGeneratedCode.Append("where ");
                mergingBuilder.AppendParameter((object)this.Name);
                mergingBuilder.myGeneratedCode.Append(" : ");
                bool isFirst = true;
                foreach (IType type in this.mySuperTypes)
                {
                    mergingBuilder.EnsureDelimiter(ref isFirst);
                    mergingBuilder.AppendParameter((object)type);
                }
                if (this.myIsClass)
                {
                    mergingBuilder.EnsureDelimiter(ref isFirst);
                    mergingBuilder.myGeneratedCode.Append("class");
                }
                if (this.myIsStruct)
                {
                    mergingBuilder.EnsureDelimiter(ref isFirst);
                    mergingBuilder.myGeneratedCode.Append("struct");
                }
                if (this.myHasConstructor)
                {
                    mergingBuilder.EnsureDelimiter(ref isFirst);
                    mergingBuilder.myGeneratedCode.Append("new()");
                }
                return true;
            }
        }

        private class TypeParameterInformation
        {
            public IList<IAttribute> Attributes;
            public TypeDeclarationMergingBuilder.TypeParameterConstraint Constraint;
        }
    }
}

