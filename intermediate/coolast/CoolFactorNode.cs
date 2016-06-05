namespace Compiler.intermediate.coolast
{
    public class CoolFactorNode : CoolAstNode
    {
        public CoolFactorNode Factor { get; }
        public CoolOpNode Op { get; }
        public CoolTermNode Term { get; }

        public CoolFactorNode(CoolFactorNode factor, CoolOpNode op, CoolTermNode term)
        {
            Factor = factor;
            Op = op;
            Term = term;
        }

        public override CoolAstNode Copy()
        {
            return new CoolFactorNode(Factor, Op, Term);
        }
    }
}