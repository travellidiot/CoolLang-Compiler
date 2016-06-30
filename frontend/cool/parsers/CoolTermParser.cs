using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolTermParser : CoolTDParser
    {
        public CoolTermParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolTermParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var valueParser = new CoolValueParser(this);
            var valueNode = valueParser.Parse();
            var args = new List<IAstNode>();
            CoolWordToken type = null;

            //var flwSet = new SortedSet<TokenType>() {TokenType.Dispatch, TokenType.Dot};
            var token = CurrentToken();
            while (token.Type.Is(TokenType.At) || token.Type.Is(TokenType.Dispatch))
            {
                if (token.Type.Is(TokenType.At))
                {
                    NextToken();
                    type = Synchronize(new SortedSet<TokenType>() {TokenType.TypeId}) as CoolWordToken;
                    NextToken();
                }
                Synchronize(new SortedSet<TokenType>() { TokenType.Dispatch });
                NextToken();
                var mName = Synchronize(new SortedSet<TokenType>() { TokenType.FuncId }) as CoolWordToken;
                NextToken();
                Synchronize(new SortedSet<TokenType>() {TokenType.LeftParen});

                var exprParser = new CoolExprParser(this);
                if (!NextToken().Type.Is(TokenType.RightParen))
                {
                    var exprNode = exprParser.Parse();
                    args.Add(exprNode);
                }

                while (!CurrentToken().Type.Is(TokenType.RightParen))
                {
                    Synchronize(new SortedSet<TokenType>() {TokenType.Comma});
                    NextToken();

                    var exprNode = exprParser.Parse();
                    args.Add(exprNode);
                }

                valueNode = new CoolTermNode(valueNode, mName, args, type);

                token = NextToken();
            }

            return valueNode;
        }
    }
}