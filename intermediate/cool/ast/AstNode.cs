namespace Compiler.intermediate.cool.ast
{
    public abstract class AstNode : IAstNode
    {
        public int LineNumber { get; set; }
        public int Position { get; set; }
        public IAstNode ParentNode { get; set; }
        public ISymbol ValType { get; set; }
        public abstract IAstNode Copy();
        public abstract void Accept(ICoolVisitor coolVisitor);
    }
}