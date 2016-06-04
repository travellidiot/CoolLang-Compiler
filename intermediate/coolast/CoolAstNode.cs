using System.Collections.Generic;

namespace Compiler.intermediate.coolast
{
    public abstract class CoolAstNode : IAstNode
    {
        public CoolAstNode ParentNode { get; set; }

        public abstract CoolAstNode Copy();
    }
}