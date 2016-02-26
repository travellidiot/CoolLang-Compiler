using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate
{
    public interface ISymTab
    {
        int NestingLevel { get; }
        ISymTabEntry Enter(string name);
        ISymTabEntry Lookup(string name);
        List<ISymTabEntry> SortedEntries();
    }
}
