using System.Collections.Generic;
using System.Text;

namespace Compiler.intermediate.cool.symtab
{
    public class MethodSymbol : SymbolScope
    {
        public List<ClassSymbol> Formals { get; } = new List<ClassSymbol>();
        public ClassSymbol RTType { get; set; }
        private readonly Stack<KeyValuePair<string, ISymbol>> _hiddenSymbols = new Stack<KeyValuePair<string, ISymbol>>();

        public MethodSymbol(string symName, IScope enclosing) : base(symName, enclosing)
        {
        }

        public void Shadow(string symName, ISymbol symbol)
        {
            ISymbol hidden;
            _hiddenSymbols.Push(Symbols.TryGetValue(symName, out hidden)
                ? new KeyValuePair<string, ISymbol>(symName, hidden)
                : new KeyValuePair<string, ISymbol>(symName, null));

            Symbols[symName] = symbol;
        }

        public void RevertHiddenSymbols()
        {
            while (_hiddenSymbols.Count != 0)
            {
                var pair = _hiddenSymbols.Pop();
                if (pair.Value == null)
                {
                    Symbols.Remove(pair.Key);
                }
                else
                {
                    Symbols[pair.Key] = pair.Value;
                }
            }
        }
    }
}