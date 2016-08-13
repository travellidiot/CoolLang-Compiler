using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class NegNode : AstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }
        public WordToken Neg { get; }

        public NegNode(IAstNode expr, WordToken neg)
        {
            Expr = expr;
            Neg = neg;
            Expr.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new NegNode(Expr.Copy(), Neg);
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }
}