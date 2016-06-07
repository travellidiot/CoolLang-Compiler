using System.Collections.Generic;
using System.Diagnostics;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolValueParser : CoolTDParser
    {
        public static SortedSet<TokenType> FirstSet = new SortedSet<TokenType>
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

        public override void Parse()
        {
            
        }

        public CoolValueNode Parse(CoolToken token)
        {
            switch (token.Type.CoolType)
            {
                case TokenType.ObjectId:
                    CoolIdNode idNode = new CoolIdNode(token as CoolWordToken);

                    var flwToken = NextToken();
                    if (flwToken.Type.Is(TokenType.Assign))
                    {
                        CoolExprParser parser = new CoolExprParser(this);
                        var exprNode = parser.Parse(NextToken());
                        return new CoolAssignNode(idNode, exprNode);
                    }
                    if (flwToken.Type.Is(TokenType.LeftParen))
                    {
                        var args = new List<CoolExprNode>();

                        var nextToken = NextToken();
                        while (!nextToken.Type.Is(TokenType.RightParen))
                        {
                            var parser = new CoolExprParser(this);
                            var exprNode = parser.Parse(CurrentToken());
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
                    var notSubNode = notSubParser.Parse(NextToken());
                    return new CoolNotNode(notSubNode);

                case TokenType.Anti:
                    var antiSubParser = new CoolExprParser(this);
                    var antiSubNode = antiSubParser.Parse(NextToken());
                    return new CoolIntCmpltNode(antiSubNode);

                case TokenType.Isvoid:
                    var ivSubParser = new CoolExprParser(this);
                    var ivSubNode = ivSubParser.Parse(NextToken());
                    return new CoolIsVoidNode(ivSubNode);

                case TokenType.LeftParen:
                    var parSubParser = new CoolExprParser(this);
                    var parSubNode = parSubParser.Parse(NextToken());
                    var parSync = Synchronize(new SortedSet<TokenType>() {TokenType.RightParen});
                    if (parSync.Type.Is(TokenType.RightParen))
                    {
                        NextToken();
                        return new CoolParenExprNode(parSubNode);
                    }
                    return null;

                case TokenType.If:
                    CoolExprNode elseNode = null;
                    var predParser = new CoolExprParser(this);
                    var preNode = predParser.Parse(NextToken());
                    var preSyncTok = Synchronize(new SortedSet<TokenType>() {TokenType.Then});

                    var thenParser = new CoolExprParser(this);
                    var thenNode = thenParser.Parse(NextToken());
                    var thenSyncTok = Synchronize(new SortedSet<TokenType>() {TokenType.Else, TokenType.Fi});

                    if (thenSyncTok.Type.Is(TokenType.Else))
                    {
                        var elseParser = new CoolExprParser(this);
                        elseNode = elseParser.Parse(NextToken());
                        var ifSyncTok = Synchronize(new SortedSet<TokenType>() {TokenType.Fi});
                    }

                    NextToken();
                    return new CoolIfNode(preNode, thenNode, elseNode);

                case TokenType.While:
                    var condParser = new CoolExprParser(this);
                    var condNode = condParser.Parse(NextToken());
                    var condSyncTok = Synchronize(new SortedSet<TokenType>() {TokenType.Loop});

                    var loopParser = new CoolExprParser(this);
                    var loopNode = loopParser.Parse(NextToken());
                    var loopSyncTok = Synchronize(new SortedSet<TokenType>() {TokenType.Pool});

                    NextToken();
                    return new CoolWhileNode(condNode, loopNode);

                default:
                    return null;
            }
        }
    }
}