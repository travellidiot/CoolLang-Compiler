namespace Compiler.intermediate.cool.symtab
{
    public class ClassSymbol : SymbolScope
    {
        public static ClassSymbol Undefined { get; } = new ClassSymbol("Undefined", null);
        public static ClassSymbol TypeClass { get; } = new ClassSymbol("Type", null);
        //public static ClassSymbol BuiltInTypeClass { get; } = new ClassSymbol("BuiltInType", null);
        public ISymbol SymType { get; set; } = TypeClass;
        public ClassSymbol Parent { get; set; }

        public ClassSymbol(string clsName, IScope enclosing) : base(clsName, enclosing)
        {
        }

        public override ISymbol Lookup(string name)
        {
            ISymbol symbol;
            _symbols.TryGetValue(name, out symbol);

            if (symbol != null)
                return symbol;

            if (Parent != null)
                symbol = Parent.Lookup(name);

            return symbol ?? EnclosingScope?.Lookup(name);
        }
    }
}