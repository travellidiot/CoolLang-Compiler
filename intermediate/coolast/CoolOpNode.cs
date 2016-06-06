using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolOpNode : CoolAstNode
    {
        public CoolSpecialToken OpToken { get; }

        public CoolOpNode(CoolSpecialToken token) { OpToken = token; }

        public override CoolAstNode Copy()
        {
            return new CoolOpNode(OpToken);
        }
    }
}