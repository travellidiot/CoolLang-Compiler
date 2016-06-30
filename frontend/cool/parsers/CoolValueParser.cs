using System;
using System.Collections.Generic;
using System.Diagnostics;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    // Tuple<CoolFormalNode, CoolExprNode>
    using Case = System.Tuple<IAstNode, IAstNode>;

    public class CoolValueParser : CoolTDParser
    {
        public static SortedSet<TokenType> ValueFirstSet = new SortedSet<TokenType>
        {
            TokenType.ObjectId,
            TokenType.FuncId,
            TokenType.IntConst,
            TokenType.StringConst,
            TokenType.BoolConst,
            TokenType.Not,
            TokenType.Anti,
            TokenType.Isvoid,
            TokenType.LeftParen,
            TokenType.If,
            TokenType.While,
            TokenType.Let,
            TokenType.Case,
            TokenType.New,
            TokenType.LeftBracket
        };

        public CoolValueParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolValueParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            CoolToken token = Synchronize(ValueFirstSet);

            switch (token.Type.CoolType)
            {
                case TokenType.ObjectId:
                    CoolIdNode idNode = new CoolIdNode(token as CoolWordToken);
                    NextToken();
                    return idNode;

                case TokenType.FuncId:
                    CoolIdNode funcNode = new CoolIdNode(token as CoolWordToken);

                    NextToken();
                    var args = new List<IAstNode>();

                    //var nextToken = NextToken();
                    if (!NextToken().Type.Is(TokenType.RightParen))
                    {
                        var parser = new CoolExprParser(this);
                        var exprNode = parser.Parse();
                        args.Add(exprNode);
                    }

                    while (!CurrentToken().Type.Is(TokenType.RightParen))
                    {
                        Synchronize(new SortedSet<TokenType>() { TokenType.Comma });
                        NextToken();

                        var parser = new CoolExprParser(this);
                        var exprNode = parser.Parse();
                        args.Add(exprNode);
                    }

                    NextToken();
                    return new CoolCallNode(funcNode, args);

                case TokenType.IntConst:
                    NextToken();
                    return new CoolIntNode(token as CoolNumberToken);

                case TokenType.StringConst:
                    NextToken();
                    return new CoolStringNode(token as CoolStringToken);

                case TokenType.BoolConst:
                    NextToken();
                    return new CoolBoolNode(token as CoolWordToken);

                case TokenType.LeftParen:
                    var parSubParser = new CoolExprParser(this);
                    NextToken();
                    var parSubNode = parSubParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.RightParen});
                    NextToken();
                    return new CoolParenExprNode(parSubNode);

                case TokenType.If:
                    IAstNode elseNode = null;
                    var exprParser = new CoolExprParser(this);
                    NextToken();
                    var preNode = exprParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.Then});

                    NextToken();
                    var thenNode = exprParser.Parse();
                    var thenSyncTok = Synchronize(new SortedSet<TokenType>() {TokenType.Else, TokenType.Fi});

                    if (thenSyncTok.Type.Is(TokenType.Else))
                    {
                        NextToken();
                        elseNode = exprParser.Parse();
                        Synchronize(new SortedSet<TokenType>() {TokenType.Fi});
                    }

                    NextToken();
                    return new CoolIfNode(preNode, thenNode, elseNode);

                case TokenType.While:
                    var whileParser = new CoolExprParser(this);
                    NextToken();
                    var condNode = whileParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.Loop});

                    NextToken();
                    var loopNode = whileParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.Pool});

                    NextToken();
                    return new CoolWhileNode(condNode, loopNode);

                case TokenType.Let:
                    var attrToken = NextToken();
                    var attrParser = new CoolFeatureParser(this);
                    var attrList = new List<CoolAttrNode>();
                    var attrNode = attrParser.Parse() as CoolAttrNode;
                    if (attrNode != null)
                        attrList.Add(attrNode);
                    else
                        ErrorHandler.Flag(attrToken, CoolErrorCode.InvalidSyntax, attrParser);

                    while (!CurrentToken().Type.Is(TokenType.In))
                    {
                        Synchronize(new SortedSet<TokenType>() {TokenType.Comma});
                        NextToken();
                        attrNode = attrParser.Parse() as CoolAttrNode;
                        if (attrNode != null)
                            attrList.Add(attrNode);
                        else
                            ErrorHandler.Flag(attrToken, CoolErrorCode.InvalidSyntax, attrParser);
                    }

                    NextToken();
                    var letBodyParser = new CoolExprParser(this);
                    var bodyExpr = letBodyParser.Parse();

                    NextToken();
                    return new CoolLetNode(attrList, bodyExpr);

                case TokenType.Case:
                    NextToken();
                    var caseParser = new CoolExprParser(this);
                    var caseNode = caseParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.Of});

                    var casesNode = new List<Case>();
                    do
                    {
                        NextToken();
                        var caseFormalParser = new CoolFormalParser(this);
                        var caseFormalNode = caseFormalParser.Parse();
                        Synchronize(new SortedSet<TokenType>() {TokenType.DArraw});

                        NextToken();
                        var caseExprParser = new CoolExprParser(this);
                        var caseExprNode = caseExprParser.Parse();
                        Synchronize(new SortedSet<TokenType>() {TokenType.Semic});

                        casesNode.Add(new Case(caseFormalNode, caseExprNode));
                    } while (!NextToken().Type.Is(TokenType.Esac));

                    NextToken();
                    return new CoolCaseNode(caseNode, casesNode);

                case TokenType.New:
                    NextToken();
                    var typeToken = Synchronize(new SortedSet<TokenType>() {TokenType.TypeId});
                    NextToken();
                    return new CoolNewObjNode(typeToken as CoolWordToken);

                case TokenType.LeftBracket:
                    var blockExprs = new List<IAstNode>();
                    NextToken();
                    var blockExprParser = new CoolExprParser(this);
                    do
                    {
                        var blockExprNode = blockExprParser.Parse();
                        blockExprs.Add(blockExprNode);

                        Synchronize(new SortedSet<TokenType>() { TokenType.Semic });
                    } while (!NextToken().Type.Is(TokenType.RightBracket));

                    NextToken();
                    return new CoolBlockNode(blockExprs);

                default:
                    return null;
            }
        }
    }
}