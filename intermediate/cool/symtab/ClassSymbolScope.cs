namespace Compiler.intermediate.cool.symtab
{
    public class ClassSymbolScope : SymbolScope, IType
    {
        public static string Undefined = "UD";
        public static ClassSymbolScope TypeClass { get; } = new ClassSymbolScope("Type", null);
        public static ClassSymbolScope BuiltInTypeClass { get; } = new ClassSymbolScope("BuiltInType", null);
        public string TypeName { get; private set; }
        public ClassSymbolScope ParentScope { get; set; }

        public ClassSymbolScope(string clsName, IScope enclosing) : base(clsName, enclosing)
        {
            TypeName = clsName;
            if (clsName == "IO" || clsName == "Object" || clsName == "Int" || clsName == "String" || clsName == "Bool")
                SymType = BuiltInTypeClass;
            else
                SymType = TypeClass;
        }

        public void DefineType(string type)
        {
            TypeName = type;
        }

        public void UndefineType()
        {
            TypeName = Undefined;
        }

        public override ISymbol Lookup(string name)
        {
            ISymbol symbol;
            _symbols.TryGetValue(name, out symbol);

            if (symbol != null)
                return symbol;

            if (ParentScope != null)
                symbol = ParentScope.Lookup(name);

            return symbol ?? EnclosingScope?.Lookup(name);
        }
    }
}