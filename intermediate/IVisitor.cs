namespace Compiler.intermediate
{
    public interface IVisitor
    {
        void Visit(IAstNode node);
    }
}