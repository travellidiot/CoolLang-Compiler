using System.Collections.Generic;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolTermNode : CoolAstNode
    {
        public CoolTermNode Term { get; }
        public CoolWordToken Type { get; }
        public CoolWordToken MethodName { get; }
        public List<CoolExprNode> Args { get; }

        public CoolTermNode(CoolTermNode term, CoolWordToken mName, List<CoolExprNode> args = null, CoolWordToken type = null)
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