using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate.symtabimpl
{
    public class SymTabStackImpl : List<ISymTab>, ISymTabStack
    {
        public int CurrentNestingLevel { get; private set; }

        public SymTabStackImpl()
        {
            CurrentNestingLevel = 0;
            Add(SymTabFactory.CreateSymTab(CurrentNestingLevel));
        }

        public ISymTab LocalSymTab
        {
            get { return this[CurrentNestingLevel]; }
        }

        public ISymTabEntry EnterLocal(string name)
        {
            return this[CurrentNestingLevel].Enter(name);
        }

        public ISymTabEntry LookupLocal(string name)
        {
            return this[CurrentNestingLevel].Lookup(name);
        }

        public ISymTabEntry Lookup(string name)
        {
            return LookupLocal(name);
        }
    }
}
