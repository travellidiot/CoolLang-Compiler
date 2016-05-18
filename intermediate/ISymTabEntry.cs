using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate
{
    public interface ISymTabEntry
    {
        string Name { get; }
        ISymTab SymTab { get; }
        void AppendLineNumber(int lineNumber);
        List<Int32> LineNumbers { get; }
        void SetAttribute(ISymTabKey key, Object value);
        Object GetAttribute(ISymTabKey key);
    }
}
