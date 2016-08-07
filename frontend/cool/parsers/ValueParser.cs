using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;
using Compiler.intermediate.cool.symtab;
using Compiler.message.cool;
using static Compiler.frontend.cool.CoolScanner;
using static Compiler.message.cool.ErrorHandler;

namespace Compiler.frontend.cool.parsers
{
    // Tuple<FormalNode, CoolExprNode>
    using Pattern = System.Tuple<IAstNode, IAstNode>;

    public class ValueParser : TdParser
    {
        public static SortedSet<ITokenType> ValueFirstSet = new SortedSet<ITokenType>()
        {
            CoolTokenType.ObjectId,
            CoolTokenType.IntConst,
            CoolTokenType.StringConst,
            CoolTokenType.BoolConst,
            CoolTokenType.Not,
            CoolTokenType.Anti,
            CoolTokenType.Isvoid,
            CoolTokenType.LeftParen,
            CoolTokenType.If,
            CoolTokenType.While,
            CoolTokenType.Let,
            CoolTokenType.Case,
            CoolTokenType.New,
            CoolTokenType.LeftBracket
        };

        public ValueParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public ValueParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var token = Synchronize(ValueFirstSet);
            if (!(token is Token))
                throw new NotCoolTokenException(token);

            var coolToken = token as Token;
            switch (coolToken.Type.CoolType)
            {
                case TokenType.ObjectId:
                    var idNode = new IdNode(coolToken as WordToken);
                    var afterId = Scanner.Lookahead(1) as Token;
                    if (afterId == null)
                        throw new NotCoolTokenException(Scanner.Lookahead(1));

                    if (afterId.Type.Is(TokenType.LeftParen))
                    {
                        /* method call*/
                        NextToken(); // eat ID
                        var args = new List<IAstNode>();

                        //var nextToken = NextToken();
                        if (!Equals(NextToken().Type, CoolTokenType.RightParen))
                        {
                            // eat "("
                            var parser = new ExprParser(this);
                            var exprNode = parser.Parse();
                            args.Add(exprNode);
                        }

                        while (!Equals(CurrentToken().Type, CoolTokenType.RightParen))
                        {
                            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma });
                            NextToken(); // eat ","

                            var parser = new ExprParser(this);
                            var exprNode = parser.Parse();
                            args.Add(exprNode);
                        }

                        NextToken(); // eat ")"
                        return new CallNode(idNode, args);
                    }

                    NextToken();
                    return idNode;

                case TokenType.IntConst:
                    NextToken();
                    return new IntNode(coolToken as NumberToken);

                case TokenType.StringConst:
                    NextToken();
                    return new StringNode(coolToken as StringToken);

                case TokenType.BoolConst:
                    NextToken();
                    return new BoolNode(coolToken as WordToken);

                case TokenType.LeftParen:
                    var parSubParser = new ExprParser(this);
                    NextToken(); // eat "("
                    var parSubNode = parSubParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.RightParen});
                    NextToken(); // eat ")" 
                    return new ParenExprNode(parSubNode);

                case TokenType.If:
                    IAstNode elseNode = null;
                    var exprParser = new ExprParser(this);
                    NextToken(); // eat "if"
                    var preNode = exprParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Then});

                    NextToken(); // eat "then"
                    var thenNode = exprParser.Parse();
                    var thenSyncTok = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Else, CoolTokenType.Fi});

                    if (Equals(thenSyncTok.Type, CoolTokenType.Else))
                    {
                        NextToken(); // eat "else"
                        elseNode = exprParser.Parse();
                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Fi});
                    }

                    NextToken(); // eat "fi"
                    return new IfNode(preNode, thenNode, elseNode);

                case TokenType.While:
                    var whileParser = new ExprParser(this);
                    NextToken(); // eat "while"
                    var condNode = whileParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Loop});

                    NextToken(); // eat "loop"
                    var loopNode = whileParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Pool});

                    NextToken(); // eat "pool"
                    return new WhileNode(condNode, loopNode);

                case TokenType.Let:
                    var attrToken = NextToken(); // eat "let"

                    var attrParser = new FeatureParser(this);
                    var attrList = new List<AttrNode>();
                    var attrNode = attrParser.Parse() as AttrNode;
                    if (attrNode != null)
                        attrList.Add(attrNode);
                    else
                        FlagSyntaxError(attrToken, ErrorCode.InvalidSyntax, attrParser);

                    while (!Equals(CurrentToken().Type, CoolTokenType.In))
                    {
                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma});
                        NextToken(); // eat ","
                        attrNode = attrParser.Parse() as AttrNode;
                        if (attrNode != null)
                            attrList.Add(attrNode);
                        else
                            FlagSyntaxError(attrToken, ErrorCode.InvalidSyntax, attrParser);
                    }

                    NextToken(); // eat "in"
                    var letBodyParser = new ExprParser(this);
                    var bodyExpr = letBodyParser.Parse();

                    return new LetNode(attrList, bodyExpr);

                case TokenType.Case:
                    NextToken(); // eat "case"
                    var caseParser = new ExprParser(this);
                    var caseNode = caseParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Of});
                    NextToken(); // eat "of"
                    var casesNode = new List<Pattern>();
                    do
                    {
                        var curTok = CurrentToken();                      

                        var caseFormalParser = new FormalParser(this);
                        var caseFormalNode = caseFormalParser.Parse();
                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.DArraw});

                        NextToken(); // eat "=>"
                        var caseExprParser = new ExprParser(this);
                        var caseExprNode = caseExprParser.Parse();
                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic});

                        casesNode.Add(new Pattern(caseFormalNode, caseExprNode));
                    } while (!Equals(NextToken().Type, CoolTokenType.Esac)); // eat ";"

                    NextToken(); // eat "esac"
                    return new PatternNode(caseNode, casesNode);

                case TokenType.New:
                    NextToken(); // eat "new"
                    var typeToken = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId});
                    NextToken(); // eat "Type;
                    return new NewObjNode(typeToken as WordToken);

                case TokenType.LeftBracket:
                    var blockExprs = new List<IAstNode>();
                    NextToken(); // eat "{"
                    var blockExprParser = new ExprParser(this);
                    do
                    {
                        var blockExprNode = blockExprParser.Parse();
                        blockExprs.Add(blockExprNode);

                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic });
                    } while (!Equals(NextToken().Type, CoolTokenType.RightBracket)); // eat ";"

                    NextToken(); // eat "}"
                    return new BlockNode(blockExprs);

                default:
                    return null;
            }
        }
    }
}