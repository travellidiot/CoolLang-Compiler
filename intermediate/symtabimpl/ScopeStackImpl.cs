using System.Collections.Generic;

namespace Compiler.intermediate.symtabimpl
{
    public class ScopeStackImpl : List<IScope>, IScopeStack
    {
        public int CurrentNestingLevel { get; }

        public ScopeStackImpl()
        {
            CurrentNestingLevel = 0;
            Add(SymTabFactory.CreateSymTab(CurrentNestingLevel));
        }

        public IScope LocalScope => this[CurrentNestingLevel];

        public IScopeEntry EnterLocal(string name)
        {
            return this[CurrentNestingLevel].Enter(name);
        }

        public IScopeEntry LookupLocal(string name)
        {
            return this[CurrentNestingLevel].Lookup(name);
        }

        public IScopeEntry Lookup(string name)
        {
            return LookupLocal(name);
        }
    }
}
