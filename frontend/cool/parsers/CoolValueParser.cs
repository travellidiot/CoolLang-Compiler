using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;
using Compiler.intermediate.coolsymtab;
using static Compiler.frontend.cool.CoolScanner;

namespace Compiler.frontend.cool.parsers
{
    // Tuple<CoolFormalNode, CoolExprNode>
    using Case = System.Tuple<IAstNode, IAstNode>;

    public class CoolValueParser : CoolTdParser
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

        public CoolValueParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolValueParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var token = Synchronize(ValueFirstSet);
            if (!(token is CoolToken))
                throw new NotCoolTokenException(token);

            var coolToken = token as CoolToken;
            switch (coolToken.Type.CoolType)
            {
                case TokenType.ObjectId:
                    var idNode = new CoolIdNode(coolToken as CoolWordToken);
                    var afterId = Scanner.Lookahead(1) as CoolToken;
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
                            var parser = new CoolExprParser(this);
                            var exprNode = parser.Parse();
                            args.Add(exprNode);
                        }

                        while (!Equals(CurrentToken().Type, CoolTokenType.RightParen))
                        {
                            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma });
                            NextToken(); // eat ","

                            var parser = new CoolExprParser(this);
                            var exprNode = parser.Parse();
                            args.Add(exprNode);
                        }

                        NextToken(); // eat ")"
                        return new CoolCallNode(idNode, args);
                    }

                    NextToken();
                    return idNode;

                case TokenType.IntConst:
                    NextToken();
                    return new CoolIntNode(coolToken as CoolNumberToken);

                case TokenType.StringConst:
                    NextToken();
                    return new CoolStringNode(coolToken as CoolStringToken);

                case TokenType.BoolConst:
                    NextToken();
                    return new CoolBoolNode(coolToken as CoolWordToken);

                case TokenType.LeftParen:
                    var parSubParser = new CoolExprParser(this);
                    NextToken(); // eat "("
                    var parSubNode = parSubParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.RightParen});
                    NextToken(); // eat ")" 
                    return new CoolParenExprNode(parSubNode);

                case TokenType.If:
                    IAstNode elseNode = null;
                    var exprParser = new CoolExprParser(this);
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
                    return new CoolIfNode(preNode, thenNode, elseNode);

                case TokenType.While:
                    var whileParser = new CoolExprParser(this);
                    NextToken(); // eat "while"
                    var condNode = whileParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Loop});

                    NextToken(); // eat "loop"
                    var loopNode = whileParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Pool});

                    NextToken(); // eat "pool"
                    return new CoolWhileNode(condNode, loopNode);

                case TokenType.Let:
                    var attrToken = NextToken(); // eat "let"

                    var outLetScope = (SymbolScope)ScopeStack.Peek();
                    var localLetScope = new SymbolScope($"{outLetScope.SymName}-Local-{attrToken.LineNumber}{attrToken.Position}", outLetScope);
                    ScopeStack.Push(localLetScope);

                    var attrParser = new CoolFeatureParser(this);
                    var attrList = new List<CoolAttrNode>();
                    var attrNode = attrParser.Parse() as CoolAttrNode;
                    if (attrNode != null)
                        attrList.Add(attrNode);
                    else
                        ErrorHandler.Flag(attrToken, CoolErrorCode.InvalidSyntax, attrParser);

                    while (!Equals(CurrentToken().Type, CoolTokenType.In))
                    {
                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma});
                        NextToken(); // eat ","
                        attrNode = attrParser.Parse() as CoolAttrNode;
                        if (attrNode != null)
                            attrList.Add(attrNode);
                        else
                            ErrorHandler.Flag(attrToken, CoolErrorCode.InvalidSyntax, attrParser);
                    }

                    NextToken(); // eat "in"
                    var letBodyParser = new CoolExprParser(this);
                    var bodyExpr = letBodyParser.Parse();

                    ScopeStack.Pop();

                    return new CoolLetNode(attrList, bodyExpr);

                case TokenType.Case:
                    NextToken(); // eat "case"
                    var caseParser = new CoolExprParser(this);
                    var caseNode = caseParser.Parse();
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Of});
                    NextToken(); // eat "of"
                    var casesNode = new List<Case>();
                    do
                    {
                        var curTok = CurrentToken();
                        var outCaseScope = (SymbolScope)ScopeStack.Peek();
                        var localCaseScope = new SymbolScope($"{outCaseScope.SymName}-Local-{curTok.LineNumber}{curTok.Position}", outCaseScope);
                        ScopeStack.Push(localCaseScope);

                        var caseFormalParser = new CoolFormalParser(this);
                        var caseFormalNode = caseFormalParser.Parse();
                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.DArraw});

                        var cNode = (CoolFormalNode) caseFormalNode;
                        var caseType = outCaseScope.LookupForType(cNode.TypeName.Text);
                        var caseId = new VariableSymbol(cNode.IdName.Text, caseType);
                        localCaseScope.Enter(caseId.SymName, caseId);

                        NextToken(); // eat "=>"
                        var caseExprParser = new CoolExprParser(this);
                        var caseExprNode = caseExprParser.Parse();
                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic});

                        ScopeStack.Pop();
                        casesNode.Add(new Case(caseFormalNode, caseExprNode));
                    } while (!Equals(NextToken().Type, CoolTokenType.Esac)); // eat ";"

                    NextToken(); // eat "esac"
                    return new CoolCaseNode(caseNode, casesNode);

                case TokenType.New:
                    NextToken(); // eat "new"
                    var typeToken = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId});
                    NextToken(); // eat "Type;
                    return new CoolNewObjNode(typeToken as CoolWordToken);

                case TokenType.LeftBracket:
                    var blockExprs = new List<IAstNode>();
                    NextToken(); // eat "{"
                    var blockExprParser = new CoolExprParser(this);
                    do
                    {
                        var blockExprNode = blockExprParser.Parse();
                        blockExprs.Add(blockExprNode);

                        Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic });
                    } while (!Equals(NextToken().Type, CoolTokenType.RightBracket)); // eat ";"

                    NextToken(); // eat "}"
                    return new CoolBlockNode(blockExprs);

                default:
                    return null;
            }
        }
    }
}