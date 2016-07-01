using System.Collections.Generic;

namespace Compiler.intermediate.symtabimpl
{
    public class ScopeEntryImpl : Dictionary<IScopeKey, object>, IScopeEntry
    {
        public ScopeEntryImpl(string name, IScope scope)
        {
            Name = name;
            Scope = scope;
            LineNumbers = new List<int>();
        }

        public List<int> LineNumbers { get; }

        public string Name { get; }

        public IScope Scope { get; }

        public void AppendLineNumber(int lineNumber)
        {
            LineNumbers.Add(lineNumber);
        }

        public object GetAttribute(IScopeKey key)
        {
            return this[key];
        }

        public void SetAttribute(IScopeKey key, object value)
        {
            Add(key, value);
        }
    }
}
