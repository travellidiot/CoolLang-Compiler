using System.Collections.Generic;

namespace Compiler.intermediate.symtabimpl
{
    public class ScopeImpl : Dictionary<string, IScopeEntry>, IScope
    {
        public ScopeImpl(int nestingLevel)
        {
            NestingLevel = nestingLevel;
        }

        public int NestingLevel { get; private set; }

        public IScopeEntry Enter(string name)
        {
            var entry = SymTabFactory.CreateSymTabEntry(name, this);
            Add(name, entry);

            return entry;
        }

        public IScopeEntry Lookup(string name)
        {
            IScopeEntry entry;
            return TryGetValue(name, out entry) ? entry : null;
        }

        public List<IScopeEntry> SortedEntries()
        {
            var list = new List<IScopeEntry>(Count);
            list.AddRange(Values);
            return list;
        }
    }
}
