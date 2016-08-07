using System.Collections.Generic;
using System.Text;

namespace Compiler.intermediate.cool.symtab
{
    public class MethodSymbol : SymbolScope
    {
        public List<ClassSymbol> Formals { get; } = new List<ClassSymbol>();
        public ClassSymbol RTType { get; set; }
        public MethodSymbol(string symName, IScope enclosing) : base(symName, enclosing)
        {
        }
    }
}