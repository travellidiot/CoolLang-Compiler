using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate.symtabimpl
{
    public class SymTabEntryImpl : Dictionary<ISymTabKey, object>, ISymTabEntry
    {
        public SymTabEntryImpl(string name, ISymTab symTab)
        {
            this.Name = name;
            this.SymTab = symTab;
            this.LineNumbers = new List<int>();
        }

        public List<int> LineNumbers { get; }

        public string Name { get; private set; }

        public ISymTab SymTab { get; private set; }

        public void appendLineNumber(int lineNumber)
        {
            LineNumbers.Add(lineNumber);
        }

        public object GetAttribute(ISymTabKey key)
        {
            return this[key];
        }

        public void SetAttribute(ISymTabKey key, object value)
        {
            Add(key, value);
        }
    }
}
