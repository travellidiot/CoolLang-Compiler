using System.Collections.Generic;

namespace Compiler.intermediate.cool.symtab
{
    public class SymbolScope : ISymbol, IScope
    {
        public string SymName { get; }
        public IType SymType { get; protected set; }
        public IScope EnclosingScope { get; }

        protected readonly Dictionary<string, ISymbol> _symbols;
        public Dictionary<string, ISymbol> Symbols => _symbols;


        public SymbolScope(string symName, IScope enclosing)
        {
            SymName = symName;
            EnclosingScope = enclosing;
            _symbols = new Dictionary<string, ISymbol>();
        }

        public ISymbol Enter(string name, ISymbol symbol)
        {
            _symbols.Add(name, symbol);
            return symbol;
        }

        public virtual ISymbol Lookup(string name)
        {
            ISymbol symbol;
            _symbols.TryGetValue(name, out symbol);

            return symbol ?? EnclosingScope?.Lookup(name);
        }

        public ClassSymbolScope LookupForType(string type)
        {
            ISymbol symbol;
            _symbols.TryGetValue(type, out symbol);

            var typeSymbol = symbol as ClassSymbolScope;

            if (typeSymbol != null)
                return typeSymbol;

            if (EnclosingScope != null)
                typeSymbol = EnclosingScope.LookupForType(type);

            if (typeSymbol != null)
                return typeSymbol;

            typeSymbol = new ClassSymbolScope(type, null);
            typeSymbol.UndefineType();

            return typeSymbol;
        }
    }
}