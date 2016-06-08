using System.Collections.Generic;
using Compiler.frontend.cool;
using Compiler.frontend.cool.tokens;


namespace Compiler.intermediate.coolast
{
    // Tuple<CoolFormalNode, CoolExprNode>
    using Case = System.Tuple<CoolAstNode, CoolAstNode>;

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
        // CoolExprNode
        public CoolAstNode Expr { get; }

        public CoolNotNode(CoolAstNode expr) { Expr = expr; }
        public override CoolAstNode Copy()
        {
            return new CoolNotNode(Expr);
        }
    }

    public class CoolIntCmpltNode : CoolValueNode
    {
        // CoolExprNode
        public CoolAstNode Expr { get; }

        public CoolIntCmpltNode(CoolAstNode expr) { Expr = expr; } 
        public override CoolAstNode Copy()
        {
            return new CoolIntCmpltNode(Expr);
        }
    }

    public class CoolIsVoidNode : CoolValueNode
    {
        // CoolExprNode
        public CoolAstNode Expr { get; }

        public CoolIsVoidNode(CoolAstNode expr) { Expr = expr; }
        public override CoolAstNode Copy()
        {
            return new CoolIsVoidNode(Expr);
        }
    }

    public class CoolParenExprNode : CoolValueNode
    {
        // CoolExprNode
        public CoolAstNode Expr { get; }

        public CoolParenExprNode(CoolAstNode expr) { Expr = expr; }
        public override CoolAstNode Copy()
        {
            return new CoolParenExprNode(Expr);
        }
    }

    public class CoolAssignNode : CoolValueNode
    {
        // CoolIdNode
        public CoolAstNode Id { get; }
        // CoolExprNode
        public CoolAstNode Expr { get; }

        public CoolAssignNode(CoolAstNode id, CoolAstNode expr)
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
        // CoolIdNode
        public CoolAstNode Id { get; }
        // List<CoolExprNode>
        public List<CoolAstNode> Args { get; }

        public CoolCallNode(CoolAstNode id, List<CoolAstNode> args)
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
        // CoolExprNode
        public CoolAstNode Pred, Expr, Alter;

        public CoolIfNode(CoolAstNode pred, CoolAstNode expr, CoolAstNode alter)
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
        // CoolExprNode
        public CoolAstNode Pred, LoopExpr;

        public CoolWhileNode(CoolAstNode pred, CoolAstNode loop)
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
        // List<CoolAttrNode>
        public List<CoolAttrNode> Attrs;
        // CoolExprNode
        public CoolAstNode Expr;

        public CoolLetNode(List<CoolAttrNode> attrs, CoolAstNode expr)
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
        // CoolExprNode
        public CoolAstNode CaseExpr;
        public List<Case> Cases;

        public CoolCaseNode(CoolAstNode expr, List<Case> cases)
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
        // CoolExprNode
        public List<CoolAstNode> Exprs;

        public CoolBlockNode(List<CoolAstNode> exprs) { Exprs = exprs; }
        public override CoolAstNode Copy()
        {
            return new CoolBlockNode(Exprs);
        }
    }
}