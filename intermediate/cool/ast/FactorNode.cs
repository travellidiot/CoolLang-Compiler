using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class FactorNode : AstNode
    {
        // FactorNode
        public IAstNode Factor { get; }
        public SpecialToken Op { get; }
        // TermNode
        public IAstNode Term { get; }

        public FactorNode(IAstNode factor, SpecialToken op = null, IAstNode term = null)
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
            return new FactorNode(Factor, Op, Term);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}