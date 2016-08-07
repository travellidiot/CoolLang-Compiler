using System.Collections.Generic;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class TermNode : AstNode
    {
        // TermNode
        public IAstNode Term { get; }
        public WordToken Type { get; }
        public WordToken MethodName { get; }
        public List<IAstNode> Args { get; }

        public TermNode(IAstNode term, WordToken mName, List<IAstNode> args = null, WordToken type = null)
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
            return new TermNode(Term, MethodName, Args, Type);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}