using Compiler.intermediate.cool;

namespace Compiler.intermediate
{
    public interface IAstNode
    {
        IAstNode ParentNode { get; set; }
        IScope Scope { get; set; }
        IAstNode Copy();
        void Accept(ICoolVisitor coolVisitor);
    }
}