using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public abstract class CoolFeatureNode : CoolAstNode
    {
    }

    public class CoolMethodNode : CoolFeatureNode
    {
        public CoolWordToken MethodName { get; }
        // List<CoolFormalNode>
        public List<CoolAstNode> Formals { get; }
        public CoolWordToken RetType { get; }
        // CoolExprNode
        public CoolAstNode Expr { get; }


        public CoolMethodNode(CoolWordToken methodName, List<CoolAstNode> formals, CoolWordToken retType, CoolAstNode expr)
        {
            MethodName = methodName;
            Formals = formals;
            RetType = retType;
            Expr = expr;

            foreach (var formal in Formals)
            {
                formal.ParentNode = this;
            }
        }
        public override CoolAstNode Copy()
        {
            var formals = Formals.Select((fm) => fm) as List<CoolAstNode>;
            var methodNode = new CoolMethodNode(MethodName, formals, RetType, Expr);

            return methodNode;
        }
    }

    public class CoolAttrNode : CoolFeatureNode
    {
        public CoolWordToken AttrName { get; }
        public CoolWordToken TypeName { get; }

        // CoolExprNode
        public CoolAstNode Expr { get; }

        public CoolAttrNode(CoolWordToken attr, CoolWordToken atype, CoolAstNode expr)
        {
            AttrName = attr;
            TypeName = atype;
            Expr = expr;
        }

        public override CoolAstNode Copy()
        {
            return new CoolAttrNode(AttrName, TypeName, Expr);
        }
    }
}