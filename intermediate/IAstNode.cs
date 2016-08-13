using System.Collections.Generic;
using Compiler.intermediate.cool;

namespace Compiler.intermediate
{
    public interface IAstNode
    {
        int LineNumber { get; set; }
        int Position { get; set; }
        IAstNode ParentNode { get; set; }
        ISymbol ValType { get; set; }
        IAstNode Copy();
        IAstNode Accept(ICoolVisitor coolVisitor);
    }
}