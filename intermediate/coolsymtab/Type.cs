namespace Compiler.intermediate.coolsymtab
{
    public class Type : IType
    {
        public string TypeName { get; }

        public Type(string name)
        {
            TypeName = name;
        }
    }
}