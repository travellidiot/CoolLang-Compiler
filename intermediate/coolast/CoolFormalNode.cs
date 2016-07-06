using Compiler.frontend.cool.tokens;
using Compiler.intermediate.coolsymtab;

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
        public override IAstNode Copy()
        {
            return new CoolFormalNode(IdName, TypeName);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}