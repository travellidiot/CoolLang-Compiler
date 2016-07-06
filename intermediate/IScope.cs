using System.Collections.Generic;

namespace Compiler.intermediate
{
    public interface IScope
    {
        ISymbol Enter(string name, ISymbol symbol);
        ISymbol Lookup(string name);
        Dictionary<string, ISymbol> Symbols { get; }
        IScope EnclosingScope { get; }
    }
}
