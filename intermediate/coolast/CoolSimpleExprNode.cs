namespace Compiler.intermediate.coolast
{
    public class CoolSimpleExprNode : CoolAstNode
    {
        public CoolSimpleExprNode SimpleExpr { get; }
        public CoolOpNode Op { get; }
        public CoolFactorNode Factor { get; }

        public CoolSimpleExprNode(CoolSimpleExprNode simpleExpr, CoolOpNode op, CoolFactorNode factor)
        {
            SimpleExpr = simpleExpr;
            Op = op;
            Factor = factor;
        }

        public override CoolAstNode Copy()
        {
            return new CoolSimpleExprNode(SimpleExpr, Op, Factor);
        }
    }
}