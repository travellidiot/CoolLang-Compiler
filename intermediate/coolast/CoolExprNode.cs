using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolExprNode : CoolAstNode
    {
        // CoolSimpleExprNode
        public IAstNode LeftSimpleExpr { get; }
        public CoolSpecialToken Op { get; }
        // CoolSimpleExprNode
        public IAstNode RightSimpleExpr { get; }
        
        public CoolExprNode(IAstNode left, CoolSpecialToken op, IAstNode right)
        {
            LeftSimpleExpr = left;
            Op = op;
            RightSimpleExpr = right;
        }

        public override IAstNode Copy()
        {
            return new CoolExprNode(left: LeftSimpleExpr, op: Op, right: RightSimpleExpr);
        }
    }
}