namespace Compiler.intermediate.cool.ast
{
    public class NotNode : AstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public NotNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new NotNode(Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}