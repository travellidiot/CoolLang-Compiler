using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public abstract class CoolFeatureNode : CoolAstNode
    {
    }

    // feature ::= ID([formal 【, formal】*]) : TYPE { expr }
    public class CoolMethodNode : CoolFeatureNode
    {
        public CoolWordToken MethodName { get; }
        // List<CoolFormalNode>
        public List<CoolFormalNode> Formals { get; }
        public CoolWordToken RetType { get; }
        // CoolExprNode
        public IAstNode Expr { get; }


        public CoolMethodNode(CoolWordToken methodName, List<CoolFormalNode> formals, CoolWordToken retType, IAstNode expr)
        {
            MethodName = methodName;
            Formals = formals;
            RetType = retType;
            Expr = expr;
            Expr.ParentNode = this;

            foreach (var formal in Formals)
            {
                formal.ParentNode = this;
            }
        }
        public override IAstNode Copy()
        {
            var formals = Formals.Select((fm) => fm) as List<CoolFormalNode>;
            var methodNode = new CoolMethodNode(MethodName, formals, RetType, Expr);

            return methodNode;
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    // feature ::= ID : TYPE[< -expr]
    public class CoolAttrNode : CoolFeatureNode
    {
        public CoolWordToken AttrName { get; }
        public CoolWordToken TypeName { get; }

        // CoolExprNode
        public IAstNode Expr { get; }

        public CoolAttrNode(CoolWordToken attr, CoolWordToken atype, IAstNode expr = null)
        {
            AttrName = attr;
            TypeName = atype;
            Expr = expr;
            if (Expr != null)
                Expr.ParentNode = this;
        }

        public override IAstNode Copy()
        {
            return new CoolAttrNode(AttrName, TypeName, Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}