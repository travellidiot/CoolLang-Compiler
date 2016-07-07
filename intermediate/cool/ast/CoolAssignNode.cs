namespace Compiler.intermediate.cool.ast
{
    public class CoolAssignNode : CoolAstNode
    {
        // CoolIdNode
        public IAstNode Id { get; }
        // CoolExprNode
        public IAstNode Expr { get; }

        public CoolAssignNode(IAstNode id, IAstNode expr)
        {
            Id = id;
            Expr = expr;
            Id.ParentNode = this;
            Expr.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new CoolAssignNode(Id, Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}