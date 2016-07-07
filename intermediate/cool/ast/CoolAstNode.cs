namespace Compiler.intermediate.cool.ast
{
    public abstract class CoolAstNode : IAstNode
    {
        public IAstNode ParentNode { get; set; }
        public IScope Scope { get; set; }
        public abstract IAstNode Copy();
        public abstract void Accept(ICoolVisitor coolVisitor);
    }
}