namespace Compiler.intermediate.symtabimpl
{
    public class ScopeKeyImpl : IScopeKey
    {
        public enum SymKey
        {
            ConstantValue,

            RoutineCode,
            RoutineSymTab,
            RoutineICode,
            RoutineParms,
            RoutineRoutines,

            DataValue
        }
        public SymKey Key { get; private set; }

        public ScopeKeyImpl(SymKey symKey)
        {
            Key = symKey;
        }
    }
}
