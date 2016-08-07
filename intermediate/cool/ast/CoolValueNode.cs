using System.Collections.Generic;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    // Tuple<FormalNode, CoolExprNode>
    using Pattern = System.Tuple<IAstNode, IAstNode>;

    //public abstract class CoolValueNode : AstNode
    //{
    //}

    public class IdNode : AstNode
    {
        public WordToken IdToken { get; }

        public IdNode(WordToken token) { IdToken = token; }
        public override IAstNode Copy()
        {
            return new IdNode(IdToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class IntNode : AstNode
    {
        public NumberToken IntToken { get; }

        public IntNode(NumberToken token) { IntToken = token; }
        public override IAstNode Copy()
        {
            return new IntNode(IntToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class StringNode : AstNode
    {
        public StringToken StringToken { get; }

        public StringNode(StringToken token) { StringToken = token; }
        public override IAstNode Copy()
        {
            return new StringNode(StringToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
    public class BoolNode : AstNode
    {
        public WordToken BoolToken { get; }

        public BoolNode(WordToken token) { BoolToken = token; }
        public override IAstNode Copy()
        {
            return new BoolNode(BoolToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class ParenExprNode : AstNode
    {
        // CoolExprNode
        public IAstNode Expr { get; }

        public ParenExprNode(IAstNode expr) { Expr = expr; Expr.ParentNode = this; }
        public override IAstNode Copy()
        {
            return new ParenExprNode(Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    

    public class CallNode : AstNode
    {
        // IdNode
        public IAstNode Id { get; }
        // List<CoolExprNode>
        public List<IAstNode> Args { get; }

        public CallNode(IAstNode id, List<IAstNode> args)
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
            return new CallNode(Id, Args);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class IfNode : AstNode
    {
        // CoolExprNode
        public IAstNode Pred, Expr, Alter;

        public IfNode(IAstNode pred, IAstNode expr, IAstNode alter)
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
            return new IfNode(Pred, Expr, Alter);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class WhileNode : AstNode
    {
        // CoolExprNode
        public IAstNode Pred, LoopExpr;

        public WhileNode(IAstNode pred, IAstNode loop)
        {
            Pred = pred;
            Pred.ParentNode = this;
            LoopExpr = loop;
            LoopExpr.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new WhileNode(Pred, LoopExpr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class LetNode : AstNode
    {
        // List<AttrNode>
        public List<AttrNode> Attrs;
        // CoolExprNode
        public IAstNode Expr;

        public LetNode(List<AttrNode> attrs, IAstNode expr)
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
            return new LetNode(Attrs, Expr);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class PatternNode : AstNode
    {
        // CoolExprNode
        public IAstNode CaseExpr;
        public List<Pattern> Cases;

        public PatternNode(IAstNode expr, List<Pattern> cases)
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
            return new PatternNode(CaseExpr, Cases);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class NewObjNode : AstNode
    {
        public WordToken TypeToken;

        public NewObjNode(WordToken token) { TypeToken = token; }
        public override IAstNode Copy()
        {
            return new NewObjNode(TypeToken);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }

    public class BlockNode : AstNode
    {
        // CoolExprNode
        public List<IAstNode> Exprs;

        public BlockNode(List<IAstNode> exprs)
        {
            Exprs = exprs;
            foreach (var expr in Exprs)
            {
                expr.ParentNode = this;
            }
        }
        public override IAstNode Copy()
        {
            return new BlockNode(Exprs);
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }
    }
}