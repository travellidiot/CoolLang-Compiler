using System.Collections.Generic;
using Compiler.intermediate.cool.symtab;

namespace Compiler.intermediate
{
    public interface IScope
    {
        ISymbol Enter(string name, ISymbol symbol);
        ISymbol Lookup(string name);
        ClassSymbolScope LookupForType(string type);
        Dictionary<string, ISymbol> Symbols { get; }
        IScope EnclosingScope { get; }
    }
}
