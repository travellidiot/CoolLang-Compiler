using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate.symtabimpl
{
    public class SymTabImpl : Dictionary<string, ISymTabEntry>, ISymTab
    {
        public SymTabImpl(int nestingLevel)
        {
            this.NestingLevel = nestingLevel;
        }

        public int NestingLevel { get; private set; }

        public ISymTabEntry Enter(string name)
        {
            var entry = SymTabFactory.CreateSymTabEntry(name, this);
            Add(name, entry);

            return entry;
        }

        public ISymTabEntry Lookup(string name)
        {
            return this[name];
        }

        public List<ISymTabEntry> SortedEntries()
        {
            var list = new List<ISymTabEntry>(this.Count);
            foreach (var entry in this.Values)
            {
                list.Add(entry);
            }
            return list;
        }
    }
}
