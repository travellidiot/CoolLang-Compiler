namespace Compiler.intermediate.coolast
{
    public class CoolFormalNode : CoolAstNode
    {
        public string IdName { get; }
        public string TypeName { get; }

        public CoolFormalNode(string id, string type)
        {
            IdName = id;
            TypeName = type;
        }
        public override CoolAstNode Copy()
        {
            return new CoolFormalNode(IdName, TypeName);
        }
    }
}