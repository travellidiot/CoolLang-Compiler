using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
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

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}