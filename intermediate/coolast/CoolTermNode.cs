using System.Collections.Generic;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolTermNode : CoolAstNode
    {
        // CoolTermNode
        public IAstNode Term { get; }
        public CoolWordToken Type { get; }
        public CoolWordToken MethodName { get; }
        public List<IAstNode> Args { get; }

        public CoolTermNode(IAstNode term, CoolWordToken mName, List<IAstNode> args = null, CoolWordToken type = null)
        {
            Term = term;
            Term.ParentNode = this;

            Type = type;
            MethodName = mName;

            Args = args;
            if (Args == null)
                return;

            foreach (var arg in Args)
            {
                arg.ParentNode = this;
            }
        }
         
        public override IAstNode Copy()
        {
            return new CoolTermNode(Term, MethodName, Args, Type);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}