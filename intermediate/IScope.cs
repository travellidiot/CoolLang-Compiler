using System.Collections.Generic;

namespace Compiler.intermediate
{
    public interface IScope
    {
        int NestingLevel { get; }
        IScopeEntry Enter(string name);
        IScopeEntry Lookup(string name);
        List<IScopeEntry> SortedEntries();
    }
}
