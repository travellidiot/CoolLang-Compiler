using System.Collections.Generic;

namespace Compiler.intermediate.cool.symtab
{
    public class MethodSymbol : SymbolScope
    {
        public List<ISymbol> TypeSignature { get; } = new List<ISymbol>();
        public MethodSymbol(string symName, IScope enclosing) : base(symName, enclosing)
        {
        }
    }
}