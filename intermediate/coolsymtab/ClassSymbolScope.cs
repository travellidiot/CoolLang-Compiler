namespace Compiler.intermediate.coolsymtab
{
    public class ClassSymbolScope : SymbolScope, IType
    {
        public static ClassSymbolScope TypeClass { get; } = new ClassSymbolScope("Type", null);
        public string TypeName { get; private set; }
        public ClassSymbolScope ParentScope { get; set; }

        public ClassSymbolScope(string clsName, IScope enclosing) : base(clsName, enclosing)
        {
            TypeName = clsName;
            SymType = TypeClass;
        }

        public void DefineType(string type)
        {
            TypeName = type;
        }

        public void UndefineType()
        {
            TypeName = "UD";
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