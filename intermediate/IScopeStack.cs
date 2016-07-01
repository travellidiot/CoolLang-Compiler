namespace Compiler.intermediate
{
    public interface IScopeStack
    {
        int CurrentNestingLevel { get; }
        IScope LocalScope { get; }
        IScopeEntry EnterLocal(string name);
        IScopeEntry LookupLocal(string name);
        IScopeEntry Lookup(string name);
    }
}
