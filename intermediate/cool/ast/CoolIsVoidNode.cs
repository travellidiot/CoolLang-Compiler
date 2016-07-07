namespace Compiler.intermediate.cool.ast
{
    public class CoolIsVoidNode : CoolAstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public CoolIsVoidNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new CoolIsVoidNode(Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}