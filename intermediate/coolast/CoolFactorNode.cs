using Compiler.frontend.cool.tokens;
using Compiler.intermediate.coolsymtab;

namespace Compiler.intermediate.coolast
{
    public class CoolFactorNode : CoolAstNode
    {
        // CoolFactorNode
        public IAstNode Factor { get; }
        public CoolSpecialToken Op { get; }
        // CoolTermNode
        public IAstNode Term { get; }

        public CoolFactorNode(IAstNode factor, CoolSpecialToken op = null, IAstNode term = null)
        {
            Factor = factor;
            Factor.ParentNode = this;
            Op = op;
            Term = term;
            if (Term != null)
                Term.ParentNode = this;
        }

        public override IAstNode Copy()
        {
            return new CoolFactorNode(Factor, Op, Term);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}