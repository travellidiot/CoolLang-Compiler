using System.Collections.Generic;

namespace Compiler.intermediate
{
    public interface IAstNode
    {
        IAstNode ParentNode { get; set; }
        IAstNode Copy();
        void Accept(IVisitor visitor);
    }
}