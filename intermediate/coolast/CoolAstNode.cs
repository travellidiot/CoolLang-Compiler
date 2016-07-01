using System.Collections.Generic;

namespace Compiler.intermediate.coolast
{
    public abstract class CoolAstNode : IAstNode
    {
        public IAstNode ParentNode { get; set; }
        public abstract IAstNode Copy();
        public abstract void Accept(IVisitor visitor);
    }
}