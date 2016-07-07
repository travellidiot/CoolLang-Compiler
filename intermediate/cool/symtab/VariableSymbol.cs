namespace Compiler.intermediate.cool.symtab
{
    public class VariableSymbol : ISymbol
    {
        public string SymName { get; }
        public IType SymType { get; set; }

        public VariableSymbol(string symName, IType symType)
        {
            SymName = symName;
            SymType = symType;
        }
    }
}