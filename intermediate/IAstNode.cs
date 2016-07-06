using System.Collections.Generic;
using Compiler.intermediate.coolsymtab;

namespace Compiler.intermediate
{
    public interface IAstNode
    {
        IAstNode ParentNode { get; set; }
        IScope Scope { get; set; }
        IAstNode Copy();
        void Accept(IVisitor visitor);
    }
}