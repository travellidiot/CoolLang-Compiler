namespace Compiler.intermediate.coolast
{
    public class CoolExprNode : CoolAstNode
    {
        public CoolSimpleExprNode LeftSimpleExpr { get; }
        public CoolOpNode Op { get; }
        public CoolSimpleExprNode RightSimpleExpr { get; }
        
        public CoolExprNode(CoolSimpleExprNode left, CoolOpNode op, CoolSimpleExprNode right)
        {
            LeftSimpleExpr = left;
            Op = op;
            RightSimpleExpr = right;
        }

        public override CoolAstNode Copy()
        {
            return new CoolExprNode(left: LeftSimpleExpr, op: Op, right: RightSimpleExpr);
        }
    }
}