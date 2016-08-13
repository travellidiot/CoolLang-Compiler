using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class RalExprNode : AstNode
    {
        // SimpleExprNode
        public IAstNode LeftSimpleExpr { get; }
        public SpecialToken Op { get; }
        // SimpleExprNode
        public IAstNode RightSimpleExpr { get; }
        
        public RalExprNode(IAstNode left, SpecialToken op, IAstNode right)
        {
            LeftSimpleExpr = left;
            LeftSimpleExpr.ParentNode = this;
            Op = op;
            RightSimpleExpr = right;
            if (RightSimpleExpr != null)
                RightSimpleExpr.ParentNode = this;
        }

        public override IAstNode Copy()
        {
            return new RalExprNode(left: LeftSimpleExpr.Copy(), op: Op, right: RightSimpleExpr.Copy());
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }
}