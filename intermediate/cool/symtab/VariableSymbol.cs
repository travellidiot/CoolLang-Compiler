namespace Compiler.intermediate.cool.symtab
{
    public class VariableSymbol : ISymbol
    {
        public string SymName { get; }
        public ISymbol SymType { get; set; }

        public VariableSymbol(string symName, ISymbol symType)
        {
            SymName = symName;
            SymType = symType;
        }
    }
}