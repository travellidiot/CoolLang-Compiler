using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolFormalNode : CoolAstNode
    {
        public CoolWordToken IdName { get; }
        public CoolWordToken TypeName { get; }

        public CoolFormalNode(CoolWordToken id, CoolWordToken type)
        {
            IdName = id;
            TypeName = type;
        }
        public override CoolAstNode Copy()
        {
            return new CoolFormalNode(IdName, TypeName);
        }
    }
}