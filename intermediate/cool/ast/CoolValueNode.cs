using System.Collections.Generic;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    // Tuple<CoolFormalNode, CoolExprNode>
    using Pattern = System.Tuple<IAstNode, IAstNode>;

    public abstract class CoolValueNode : CoolAstNode
    {

    }

    public class CoolIdNode : CoolValueNode
    {
        public CoolWordToken IdToken { get; }

        public CoolIdNode(CoolWordToken token) { IdToken = token; }
        public override IAstNode Copy()
        {
            return new CoolIdNode(IdToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolIntNode : CoolValueNode
    {
        public CoolNumberToken IntToken { get; }

        public CoolIntNode(CoolNumberToken token) { IntToken = token; }
        public override IAstNode Copy()
        {
            return new CoolIntNode(IntToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolStringNode : CoolValueNode
    {
        public CoolStringToken StringToken { get; }

        public CoolStringNode(CoolStringToken token) { StringToken = token; }
        public override IAstNode Copy()
        {
            return new CoolStringNode(StringToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
    public class CoolBoolNode : CoolValueNode
    {
        public CoolWordToken BoolToken { get; }

        public CoolBoolNode(CoolWordToken token) { BoolToken = token; }
        public override IAstNode Copy()
        {
            return new CoolBoolNode(BoolToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolParenExprNode : CoolValueNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public CoolParenExprNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new CoolParenExprNode(Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    

    public class CoolCallNode : CoolValueNode
    {
        // CoolIdNode
        public IAstNode Id { get; }
        // List<CoolExprNode>
        public List<IAstNode> Args { get; }

        public CoolCallNode(IAstNode id, List<IAstNode> args)
        {
            Id = id;
            Id.ParentNode = this;

            Args = args;
            if (Args == null)
                return;
            foreach (var arg in Args)
            {
                arg.ParentNode = this;
            }
        } 
        public override IAstNode Copy()
        {
            return new CoolCallNode(Id, Args);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolIfNode : CoolValueNode
    {
        // CoolExprNode
        public IAstNode Pred, Expr, Alter;

        public CoolIfNode(IAstNode pred, IAstNode expr, IAstNode alter)
        {
            Pred = pred;
            Pred.ParentNode = this;
            Expr = expr;
            Expr.ParentNode = this;
            Alter = alter;
            if (Alter != null)
                Alter.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new CoolIfNode(Pred, Expr, Alter);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolWhileNode : CoolValueNode
    {
        // CoolExprNode
        public IAstNode Pred, LoopExpr;

        public CoolWhileNode(IAstNode pred, IAstNode loop)
        {
            Pred = pred;
            Pred.ParentNode = this;
            LoopExpr = loop;
            LoopExpr.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new CoolWhileNode(Pred, LoopExpr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolLetNode : CoolValueNode
    {
        // List<CoolAttrNode>
        public List<CoolAttrNode> Attrs;
        // CoolExprNode
        public IAstNode Expr;

        public CoolLetNode(List<CoolAttrNode> attrs, IAstNode expr)
        {
            Attrs = attrs;
            Expr = expr;
            Expr.ParentNode = this;

            foreach (var attr in Attrs)
            {
                attr.ParentNode = this;
            }
        }
        public override IAstNode Copy()
        {
            return new CoolLetNode(Attrs, Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolPatternNode : CoolValueNode
    {
        // CoolExprNode
        public IAstNode CaseExpr;
        public List<Pattern> Cases;

        public CoolPatternNode(IAstNode expr, List<Pattern> cases)
        {
            CaseExpr = expr;
            CaseExpr.ParentNode = this;
            Cases = cases;
            foreach (var cs in Cases)
            {
                cs.Item1.ParentNode = this;
                cs.Item2.ParentNode = this;
            }
        }
        public override IAstNode Copy()
        {
            return new CoolPatternNode(CaseExpr, Cases);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolNewObjNode : CoolValueNode
    {
        public CoolWordToken TypeToken;

        public CoolNewObjNode(CoolWordToken token) { TypeToken = token; }
        public override IAstNode Copy()
        {
            return new CoolNewObjNode(TypeToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class CoolBlockNode : CoolValueNode
    {
        // CoolExprNode
        public List<IAstNode> Exprs;

        public CoolBlockNode(List<IAstNode> exprs)
        {
            Exprs = exprs;
            foreach (var expr in Exprs)
            {
                expr.ParentNode = this;
            }
        }
        public override IAstNode Copy()
        {
            return new CoolBlockNode(Exprs);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}