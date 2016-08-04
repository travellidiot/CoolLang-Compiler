using System.Collections.Generic;

namespace Compiler.intermediate.cool.symtab
{
    public class SymbolScope : ISymbol, IScope
    {
        public string SymName { get; protected set; }
        public IScope EnclosingScope { get; }

        // ReSharper disable once InconsistentNaming
        protected readonly Dictionary<string, ISymbol> _symbols;
        public Dictionary<string, ISymbol> Symbols => _symbols;


        public SymbolScope(string symName, IScope enclosing)
        {
            SymName = symName;
            EnclosingScope = enclosing;
            _symbols = new Dictionary<string, ISymbol>();
        }

        public virtual ISymbol Enter(string name, ISymbol symbol)
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

        public ClassSymbol LookupForType(string type)
        {
            ISymbol symbol;
            _symbols.TryGetValue(type, out symbol);

            var typeSymbol = symbol as ClassSymbol;

            if (typeSymbol != null)
                return typeSymbol;

            if (EnclosingScope != null)
                typeSymbol = EnclosingScope.LookupForType(type);

            return typeSymbol;
        }
    }
}