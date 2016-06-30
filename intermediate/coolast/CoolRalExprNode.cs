using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolRalExprNode : CoolAstNode
    {
        // CoolSimpleExprNode
        public IAstNode LeftSimpleExpr { get; }
        public CoolSpecialToken Op { get; }
        // CoolSimpleExprNode
        public IAstNode RightSimpleExpr { get; }
        
        public CoolRalExprNode(IAstNode left, CoolSpecialToken op, IAstNode right)
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
            return new CoolRalExprNode(left: LeftSimpleExpr, op: Op, right: RightSimpleExpr);
        }
    }
}