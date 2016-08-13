namespace Compiler.intermediate.cool.ast
{
    public class IsVoidNode : AstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public IsVoidNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new IsVoidNode(Expr.Copy());
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }
}