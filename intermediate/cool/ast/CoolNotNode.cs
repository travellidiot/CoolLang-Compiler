namespace Compiler.intermediate.cool.ast
{
    public class CoolNotNode : CoolAstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public CoolNotNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new CoolNotNode(Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}