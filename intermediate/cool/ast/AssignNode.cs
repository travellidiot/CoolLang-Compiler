namespace Compiler.intermediate.cool.ast
{
    public class AssignNode : AstNode
    {
        // IdNode
        public IAstNode Id { get; }
        // CoolExprNode
        public IAstNode Expr { get; }

        public AssignNode(IAstNode id, IAstNode expr)
        {
            Id = id;
            Expr = expr;
            Id.ParentNode = this;
            Expr.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new AssignNode(Id, Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}