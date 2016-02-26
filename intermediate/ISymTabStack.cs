using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate
{
    public interface ISymTabStack
    {
        Int32 CurrentNestingLevel { get; }
        ISymTab LocalSymTab { get; }
        ISymTabEntry EnterLocal(string name);
        ISymTabEntry LookupLocal(string name);
        ISymTabEntry Lookup(string name);
    }
}
