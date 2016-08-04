using System.Collections.Generic;
using Compiler.intermediate.cool;

namespace Compiler.intermediate
{
    public interface IAstNode
    {
        IScope LocalScope { get; set; }
        IAstNode ParentNode { get; set; }
        IScope Scope { get; set; }
        IAstNode Copy();
        void Accept(ICoolVisitor coolVisitor);
    }
}