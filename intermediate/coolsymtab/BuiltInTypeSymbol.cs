namespace Compiler.intermediate.coolsymtab
{
    public class BuiltInTypeSymbol : ISymbol, IType
    {
        public BuiltInTypeSymbol(string typeName)
        {
            SymName = typeName;
            TypeName = typeName;
            SymType = new Type("BuiltInType");
        }

        public string SymName { get; }
        public IType SymType { get; }
        public string TypeName { get; }
    }
}