using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class NotNode : AstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }
        public WordToken NotToken { get; }

        public NotNode(IAstNode expr, WordToken not) { Expr = expr; NotToken = not; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new NotNode(Expr.Copy(), NotToken);
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }
}