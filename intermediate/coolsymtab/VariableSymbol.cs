namespace Compiler.intermediate.coolsymtab
{
    public class VariableSymbol : ISymbol
    {
        public string SymName { get; }
        public IType SymType { get; }

        public VariableSymbol(string symName, IType symType)
        {
            SymName = symName;
            SymType = symType;
        }
    }
}