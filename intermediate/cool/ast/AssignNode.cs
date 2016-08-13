using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class AssignNode : AstNode
    {
        // IdNode
        public IAstNode Id { get; }
        // CoolExprNode
        public IAstNode Expr { get; }
        public SpecialToken AssignToken { get; }

        public AssignNode(IAstNode id, IAstNode expr, SpecialToken assignToken)
        {
            Id = id;
            Expr = expr;
            AssignToken = assignToken;
            Id.ParentNode = this;
            Expr.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new AssignNode(Id.Copy(), Expr.Copy(), AssignToken);
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }
}