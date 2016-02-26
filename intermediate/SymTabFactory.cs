using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.intermediate.symtabimpl;

namespace Compiler.intermediate
{
    public class SymTabFactory
    {
        public static ISymTabStack CreateSymTabStack()
        {
            return new SymTabStackImpl();
        }

        public static ISymTab CreateSymTab(int nestingLevel)
        {
            return new SymTabImpl(nestingLevel);
        }

        public static ISymTabEntry CreateSymTabEntry(string name, ISymTab symTab)
        {
            return new SymTabEntryImpl(name, symTab);
        }
    }
}
