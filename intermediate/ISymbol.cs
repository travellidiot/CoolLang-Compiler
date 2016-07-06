namespace Compiler.intermediate
{
    public interface ISymbol
    {
        string SymName { get; }
        IType SymType { get; } 
    }
}