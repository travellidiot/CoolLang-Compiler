using System;
using System.Collections.Generic;
using System.Diagnostics;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    // Tuple<CoolFormalNode, CoolExprNode>
    using Case = System.Tuple<CoolAstNode, CoolAstNode>;

    public class CoolValueParser : CoolTDParser
    {
        public static SortedSet<TokenType> ValueFirstSet = new SortedSet<TokenType>
        {
            TokenType.ObjectId,
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

        public CoolValueParser(CoolTDParser parent) : base(parent.Scanner)
        {
        }

        public override CoolAstNode Parse()
        {
            CoolToken token = Synchronize(ValueFirstSet);
            switch (token.Type.CoolType)
            {
                case TokenType.ObjectId:
                    CoolIdNode idNode = new CoolIdNode(token as CoolWordToken);

                    var flwToken = NextToken();
                    if (flwToken.Type.Is(TokenType.Assign))
                    {
                        CoolExprParser parser = new CoolExprParser(this);
                        NextToken();
                        var exprNode = parser.Parse() as CoolExprNode;
                        return new CoolAssignNode(idNode, exprNode);
                    }
                    if (flwToken.Type.Is(TokenType.LeftParen))
                    {
                        var args = new List<CoolAstNode>();

                        var nextToken = NextToken();
                        while (!nextToken.Type.Is(TokenType.RightParen))
                        {
                            var parser = new CoolExprParser(this);
                            NextToken();
                            var exprNode = parser.Parse() as CoolExprNode;
                            args.Add(exprNode);

                            var syncSet = new SortedSet<TokenType> {TokenType.Comma, TokenType.RightParen};
                            nextToken = Synchronize(syncSet);
                        }

                        NextToken();
                        return new CoolCallNode(idNode, args);
                    }
                    return idNode;

                case TokenType.IntConst:
                    NextToken();
                    return new CoolIntNode(token as CoolNumberToken);

                case TokenType.StringConst:
                    NextToken();
                    return new CoolStringNode(token as CoolStringToken);

                case TokenType.BoolConst:
                    NextToken();
                    return new CoolBoolNode(token as CoolWordToken);

                case TokenType.Not:
                    var notSubParser = new CoolExprParser(this);
                    NextToken();
                    var notSubNode = notSubParser.Parse();
                    return new CoolNotNode(notSubNode);

                case TokenType.Anti:
                    var antiSubParser = new CoolExprParser(this);
                    NextToken();
                    var antiSubNode = antiSubParser.Parse();
                    return new CoolIntCmpltNode(antiSubNode);

                case TokenType.Isvoid:
                    var ivSubParser = new CoolExprParser(this);
                    NextToken();
                    var ivSubNode = ivSubParser.Parse();
                    return new CoolIsVoidNode(ivSubNode);

                case TokenType.LeftParen:
                    var parSubParser = new CoolExprParser(this);
                    NextToken();
                    var parSubNode = parSubParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.RightParen});
                    NextToken();
                    return new CoolParenExprNode(parSubNode);

                case TokenType.If:
                    CoolAstNode elseNode = null;
                    var predParser = new CoolExprParser(this);
                    NextToken();
                    var preNode = predParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.Then});

                    var thenParser = new CoolExprParser(this);
                    NextToken();
                    var thenNode = thenParser.Parse();
                    var thenSyncTok = Synchronize(new SortedSet<TokenType>() {TokenType.Else, TokenType.Fi});

                    if (thenSyncTok.Type.Is(TokenType.Else))
                    {
                        var elseParser = new CoolExprParser(this);
                        NextToken();
                        elseNode = elseParser.Parse();
                        Synchronize(new SortedSet<TokenType>() {TokenType.Fi});
                    }

                    NextToken();
                    return new CoolIfNode(preNode, thenNode, elseNode);

                case TokenType.While:
                    var condParser = new CoolExprParser(this);
                    NextToken();
                    var condNode = condParser.Parse();
                    Synchronize(new SortedSet<TokenType>() {TokenType.Loop});

                    var loopParser = new CoolExprParser(this);
                    NextToken();
                    var loopNode = loopParser.Parse();
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
                    var blockExprs = new List<CoolAstNode>();
                    NextToken();
                    do
                    {
                        var blockExprParser = new CoolExprParser(this);
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