using System.Collections.Generic;
using Compiler.frontend.cool;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
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

    public class CoolIfNode : CoolValueNode { }

    public class CoolWhileNode : CoolValueNode { }

    public class CoolLetNode : CoolValueNode { }

    public class CoolCaseNode : CoolValueNode { }

    public class CoolNewObjNode : CoolValueNode { }

    public class CoolBlockNode : CoolValueNode { }
}