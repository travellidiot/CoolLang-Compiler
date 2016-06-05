using System.Collections.Generic;

namespace Compiler.intermediate.coolast
{
    public class CoolTermNode : CoolAstNode
    {
        public CoolTermNode Term { get; }
        public string Type { get; }
        public string MethodName { get; }
        public List<CoolExprNode> Args { get; }

        public CoolTermNode(CoolTermNode term, string mName, List<CoolExprNode> args = null, string type = null)
        {
            Term = term;
            Type = type;
            MethodName = mName;
            Args = args;
        }
         
        public override CoolAstNode Copy()
        {
            return new CoolTermNode(Term, MethodName, Args, Type);
        }
    }
}