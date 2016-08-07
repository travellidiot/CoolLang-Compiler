using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class FormalNode : AstNode
    {
        public WordToken IdName { get; }
        public WordToken TypeName { get; }

        public FormalNode(WordToken id, WordToken type)
        {
            IdName = id;
            TypeName = type;
        }
        public override IAstNode Copy()
        {
            return new FormalNode(IdName, TypeName);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}