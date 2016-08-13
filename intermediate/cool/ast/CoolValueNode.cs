using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    // Tuple<FormalNode, CoolExprNode>
    using Pattern = System.Tuple<FormalNode, IAstNode>;

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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }

    

    public class CallNode : AstNode
    {
        // IdNode
        public WordToken Id { get; }
        // List<CoolExprNode>
        public List<IAstNode> Args { get; }

        public CallNode(WordToken id, List<IAstNode> args)
        {
            Id = id;
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
            return new CallNode(Id, Args.Select(arg => arg.Copy()).ToList());
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }

    public class IfNode : AstNode
    {
        // CoolExprNode
        public IAstNode Pred, Expr, Alter;
        public WordToken IfToken { get; }

        public IfNode(WordToken ifToken, IAstNode pred, IAstNode expr, IAstNode alter)
        {
            IfToken = ifToken;
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
            return new IfNode(IfToken, Pred.Copy(), Expr.Copy(), Alter.Copy());
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }

    public class WhileNode : AstNode
    {
        // CoolExprNode
        public IAstNode Pred, LoopExpr;
        public WordToken WhileToken;

        public WhileNode(WordToken wToken, IAstNode pred, IAstNode loop)
        {
            WhileToken = wToken;
            Pred = pred;
            Pred.ParentNode = this;
            LoopExpr = loop;
            LoopExpr.ParentNode = this;
        }
        public override IAstNode Copy()
        {
            return new WhileNode(WhileToken, Pred.Copy(), LoopExpr.Copy());
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
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

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }
    }
}