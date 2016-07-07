using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolTermParser : CoolTdParser
    {
        public CoolTermParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolTermParser(CoolTdParser parent) : base(parent)
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
            while (Equals(token.Type, CoolTokenType.At) || Equals(token.Type, CoolTokenType.Dispatch))
            {
                if (Equals(token.Type, CoolTokenType.At))
                {
                    NextToken(); // eat "@"
                    type = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId}) as CoolWordToken;
                    NextToken(); // eat "TYPE", usually base type name
                }

                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Dispatch });
                NextToken(); // eat "."
                var mName = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.ObjectId }) as CoolWordToken;
                NextToken(); // eat method name
                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.LeftParen});

                var exprParser = new CoolExprParser(this);
                if (!Equals(NextToken().Type, CoolTokenType.RightParen))
                {
                    // eat "("
                    var exprNode = exprParser.Parse();
                    args.Add(exprNode);
                }

                // current token is "," or ")"
                while (!Equals(CurrentToken().Type, CoolTokenType.RightParen))
                {
                    Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma});
                    NextToken(); // eat ","

                    var exprNode = exprParser.Parse();
                    args.Add(exprNode);
                }

                valueNode = new CoolTermNode(valueNode, mName, args, type);

                token = NextToken(); // eat ")"
            }

            return valueNode;
        }
    }
}