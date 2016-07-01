using System;
using System.Collections.Generic;

namespace Compiler.intermediate
{
    public interface IScopeEntry
    {
        string Name { get; }
        IScope Scope { get; }
        void AppendLineNumber(int lineNumber);
        List<Int32> LineNumbers { get; }
        void SetAttribute(IScopeKey key, Object value);
        Object GetAttribute(IScopeKey key);
    }
}
