using System.Collections.Generic;
using System.Linq;

namespace Compiler.intermediate.coolast
{
    public abstract class CoolFeatureNode : CoolAstNode
    {
    }

    public class CoolMethodNode : CoolFeatureNode
    {
        public string MethodName { get; }
        public List<CoolFormalNode> Formals { get; }
        public string RetType { get; }
        public CoolExprNode Expr { get; }


        public CoolMethodNode(string methodName, List<CoolFormalNode> formals, string retType, CoolExprNode expr)
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
            var formals = Formals.Select((fm) => fm) as List<CoolFormalNode>;
            var methodNode = new CoolMethodNode(MethodName, formals, RetType, Expr);

            return methodNode;
        }
    }

    public class CoolAttrNode : CoolFeatureNode
    {
        public string AttrName { get; }
        public string TypeName { get; }
        public CoolExprNode Expr { get; }

        public CoolAttrNode(string attr, string atype, CoolExprNode expr)
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