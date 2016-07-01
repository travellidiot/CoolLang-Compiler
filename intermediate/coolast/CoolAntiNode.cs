namespace Compiler.intermediate.coolast
{
    public class CoolAntiNode : CoolAstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public CoolAntiNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new CoolAntiNode(Expr);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}