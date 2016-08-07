using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class SimpleExprNode : AstNode
    {
        // SimpleExprNode
        public IAstNode SimpleExpr { get; }
        public SpecialToken Op { get; }
        // FactorNode
        public IAstNode Factor { get; }

        public SimpleExprNode(IAstNode simpleExpr, SpecialToken op, IAstNode factor)
        {
            SimpleExpr = simpleExpr;
            SimpleExpr.ParentNode = this;
            Op = op;
            Factor = factor;
            if (Factor != null)
                Factor.ParentNode = this;
        }

        public override IAstNode Copy()
        {
            return new SimpleExprNode(SimpleExpr, Op, Factor);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}