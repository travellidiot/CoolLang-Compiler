using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public abstract class FeatureNode : AstNode
    {
    }

    // feature ::= ID([formal 【, formal】*]) : TYPE { expr }
    public class MethodNode : FeatureNode
    {
        public WordToken MethodName { get; }
        public string MangledName { get; }
        // List<FormalNode>
        public List<FormalNode> Formals { get; }
        public WordToken RetType { get; }
        // CoolExprNode
        public IAstNode Expr { get; }


        public MethodNode(WordToken methodName, List<FormalNode> formals, WordToken retType, IAstNode expr)
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

            MangledName = MangleName();
        }

        private string MangleName()
        {
            var builder = new StringBuilder();
            builder.Append(MethodName.Text);
            builder.Append("_");

            foreach (var formal in Formals)
            {
                builder.Append("_");
                builder.Append(formal.TypeName.Text);
            }
            return builder.ToString();
        }
        public override IAstNode Copy()
        {
            var formals = Formals.Select((fm) => fm) as List<FormalNode>;
            var methodNode = new MethodNode(MethodName, formals, RetType, Expr);

            return methodNode;
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    // feature ::= ID : TYPE[< -expr]
    public class AttrNode : FeatureNode
    {
        public WordToken AttrName { get; }
        public WordToken TypeName { get; }

        // CoolExprNode
        public IAstNode Expr { get; }

        public AttrNode(WordToken attr, WordToken atype, IAstNode expr = null)
        {
            AttrName = attr;
            TypeName = atype;
            Expr = expr;
            if (Expr != null)
                Expr.ParentNode = this;
        }

        public override IAstNode Copy()
        {
            return new AttrNode(AttrName, TypeName, Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}