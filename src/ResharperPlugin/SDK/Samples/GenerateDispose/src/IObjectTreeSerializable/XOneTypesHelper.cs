using JetBrains.ReSharper.Feature.Services.CSharp.Generate;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
    internal class XOneTypesHelper
    {
        //TODO : change namespace
        private const string Namespace = "ResharperPluginTestProject";

        public static bool HasIObjectTreeSerializable(CSharpGeneratorContext context)
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

        public static ITypeElement GetIObjectTreeSerializableInterface(IGeneratorContext context)
        {
            return TypeFactory.CreateTypeByCLRName(string.Format("{0}.IObjectTreeSerializable", Namespace), context.PsiModule, context.Anchor.GetResolveContext()).GetTypeElement();
        }

        public static IType GetIObjectTreeInterface(IGeneratorContext context)
        {
            return TypeFactory.CreateTypeByCLRName(string.Format("{0}.IObjectTree", Namespace), context.PsiModule, context.Anchor.GetResolveContext()).ToIType();
        }
    }
}