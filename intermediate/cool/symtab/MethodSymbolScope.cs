using System.Collections.Generic;

namespace Compiler.intermediate.cool.symtab
{
    public class MethodSymbolScope : SymbolScope
    {
        public List<IType> TypeSignature { get; } = new List<IType>();  
        public MethodSymbolScope(string symName, IScope enclosing) : base(symName, enclosing)
        {
        }
    }
}