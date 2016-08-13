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
            return new FactorNode(Factor.Copy(), Op, Term.Copy());
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }
}