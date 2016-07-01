using Compiler.intermediate.symtabimpl;

namespace Compiler.intermediate
{
    public class SymTabFactory
    {
        public static IScopeStack CreateSymTabStack()
        {
            return new ScopeStackImpl();
        }

        public static IScope CreateSymTab(int nestingLevel)
        {
            return new ScopeImpl(nestingLevel);
        }

        public static IScopeEntry CreateSymTabEntry(string name, IScope scope)
        {
            return new ScopeEntryImpl(name, scope);
        }
    }
}
