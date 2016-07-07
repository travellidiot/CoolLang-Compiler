namespace Compiler.intermediate.cool.symtab
{
    public class BuiltInTypeSymbol : SymbolScope, IType
    {
        public BuiltInTypeSymbol(string typeName) : base (typeName, null) 
        {
            TypeName = typeName;
            SymType = new ClassSymbolScope("Type", null);
        }
        public string TypeName { get; }
    }
}