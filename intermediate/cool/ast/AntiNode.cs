namespace Compiler.intermediate.cool.ast
{
    public class AntiNode : AstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public AntiNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new AntiNode(Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}