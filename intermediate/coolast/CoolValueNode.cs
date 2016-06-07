using System.Collections.Generic;
using Compiler.frontend.cool;
using Compiler.frontend.cool.tokens;


namespace Compiler.intermediate.coolast
{
    using Case = System.Tuple<CoolFormalNode, CoolExprNode>;

    public abstract class CoolValueNode : CoolAstNode
    {
    }

    public class CoolIdNode : CoolValueNode
    {
        public CoolWordToken IdToken { get; }

        public CoolIdNode(CoolWordToken token) { IdToken = token; }
        public override CoolAstNode Copy()
        {
            return new CoolIdNode(IdToken);
        }
    }

    public class CoolIntNode : CoolValueNode
    {
        public CoolNumberToken IntToken { get; }

        public CoolIntNode(CoolNumberToken token) { IntToken = token; }
        public override CoolAstNode Copy()
        {
            return new CoolIntNode(IntToken);
        }
    }

    public class CoolStringNode : CoolValueNode
    {
        public CoolStringToken StringToken { get; }

        public CoolStringNode(CoolStringToken token) { StringToken = token; }
        public override CoolAstNode Copy()
        {
            return new CoolStringNode(StringToken);
        }
    }
    public class CoolBoolNode : CoolValueNode
    {
        public CoolWordToken BoolToken { get; }

        public CoolBoolNode(CoolWordToken token) { BoolToken = token; }
        public override CoolAstNode Copy()
        {
            return new CoolBoolNode(BoolToken);
        }
    }

    public class CoolNotNode : CoolValueNode
    {
        public CoolExprNode Expr { get; }

        public CoolNotNode(CoolExprNode expr) { Expr = expr; }
        public override CoolAstNode Copy()
        {
            return new CoolNotNode(Expr);
        }
    }

    public class CoolIntCmpltNode : CoolValueNode
    {
        public CoolExprNode Expr { get; }

        public CoolIntCmpltNode(CoolExprNode expr) { Expr = expr; } 
        public override CoolAstNode Copy()
        {
            return new CoolIntCmpltNode(Expr);
        }
    }

    public class CoolIsVoidNode : CoolValueNode
    {
        public CoolExprNode Expr { get; }

        public CoolIsVoidNode(CoolExprNode expr) { Expr = expr; }
        public override CoolAstNode Copy()
        {
            return new CoolIsVoidNode(Expr);
        }
    }

    public class CoolParenExprNode : CoolValueNode
    {
        public CoolExprNode Expr { get; }

        public CoolParenExprNode(CoolExprNode expr) { Expr = expr; }
        public override CoolAstNode Copy()
        {
            return new CoolParenExprNode(Expr);
        }
    }

    public class CoolAssignNode : CoolValueNode
    {
        public CoolIdNode Id { get; }
        public CoolExprNode Expr { get; }

        public CoolAssignNode(CoolIdNode id, CoolExprNode expr)
        {
            Id = id;
            Expr = expr;
        }
        public override CoolAstNode Copy()
        {
            return new CoolAssignNode(Id, Expr);
        }
    }

    public class CoolCallNode : CoolValueNode
    {
        public CoolIdNode Id { get; }
        public List<CoolExprNode> Args { get; }

        public CoolCallNode(CoolIdNode id, List<CoolExprNode> args)
        {
            Id = id;
            Args = args;
        } 
        public override CoolAstNode Copy()
        {
            return new CoolCallNode(Id, Args);
        }
    }

    public class CoolIfNode : CoolValueNode
    {
        public CoolExprNode Pred, Expr, Alter;

        public CoolIfNode(CoolExprNode pred, CoolExprNode expr, CoolExprNode alter)
        {
            Pred = pred;
            Expr = expr;
            Alter = alter;
        }
        public override CoolAstNode Copy()
        {
            return new CoolIfNode(Pred, Expr, Alter);
        }
    }

    public class CoolWhileNode : CoolValueNode
    {
        public CoolExprNode Pred, LoopExpr;

        public CoolWhileNode(CoolExprNode pred, CoolExprNode loop)
        {
            Pred = pred;
            LoopExpr = loop;
        }
        public override CoolAstNode Copy()
        {
            return new CoolWhileNode(Pred, LoopExpr);
        }
    }

    public class CoolLetNode : CoolValueNode
    {
        public List<CoolAttrNode> Attrs;
        public CoolExprNode Expr;

        public CoolLetNode(List<CoolAttrNode> attrs, CoolExprNode expr)
        {
            Attrs = attrs;
            Expr = expr;
        }
        public override CoolAstNode Copy()
        {
            return new CoolLetNode(Attrs, Expr);
        }
    }

    public class CoolCaseNode : CoolValueNode
    {
        public CoolExprNode CaseExpr;
        public List<Case> Cases;

        public CoolCaseNode(CoolExprNode expr, List<Case> cases)
        {
            CaseExpr = expr;
            Cases = cases;
        }
        public override CoolAstNode Copy()
        {
            return new CoolCaseNode(CaseExpr, Cases);
        }
    }

    public class CoolNewObjNode : CoolValueNode
    {
        public CoolWordToken TypeToken;

        public CoolNewObjNode(CoolWordToken token) { TypeToken = token; }
        public override CoolAstNode Copy()
        {
            return new CoolNewObjNode(TypeToken);
        }
    }

    public class CoolBlockNode : CoolValueNode
    {
        public List<CoolExprNode> Exprs;

        public CoolBlockNode(List<CoolExprNode> exprs) { Exprs = exprs; }
        public override CoolAstNode Copy()
        {
            return new CoolBlockNode(Exprs);
        }
    }
}