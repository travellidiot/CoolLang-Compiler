using System.Collections.Generic;

namespace Compiler.intermediate.coolast
{
    public abstract class CoolAstNode : IAstNode
    {

        public abstract IAstNode Copy();
        public IAstNode ParentNode { get; set; }

        public void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}