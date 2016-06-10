using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolFactorNode : CoolAstNode
    {
        // CoolFactorNode
        public IAstNode Factor { get; }
        public CoolSpecialToken Op { get; }
        // CoolTermNode
        public IAstNode Term { get; }

        public CoolFactorNode(IAstNode factor, CoolSpecialToken op = null, IAstNode term = null)
        {
            Factor = factor;
            Op = op;
            Term = term;
        }

        public override IAstNode Copy()
        {
            return new CoolFactorNode(Factor, Op, Term);
        }
    }
}