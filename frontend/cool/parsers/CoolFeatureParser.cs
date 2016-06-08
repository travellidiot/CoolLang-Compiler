using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolFeatureParser : CoolTDParser
    {
        public CoolFeatureParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFeatureParser(CoolTDParser parent) : base(parent)
        {
        }

        public override CoolAstNode Parse()
        {
            var token = Synchronize(new SortedSet<TokenType>() { TokenType.ObjectId });
            NextToken();
            var flwToken = Synchronize(new SortedSet<TokenType>() { TokenType.LeftParen, TokenType.Colon });

            // Attribute
            if (flwToken.Type.Is(TokenType.Colon))
            {
                NextToken();
                var typeToken = Synchronize(new SortedSet<TokenType>() { TokenType.TypeId });
                NextToken();
                var ctnToken = Synchronize(new SortedSet<TokenType>() { TokenType.Assign, TokenType.Semic });
                CoolAstNode exprNode = null;
                if (ctnToken.Type.Is(TokenType.Assign))
                {
                    var exprParser = new CoolExprParser(this);
                    exprNode = exprParser.Parse();
                }
                else
                {
                    NextToken();
                }
                return new CoolAttrNode(token as CoolWordToken, typeToken as CoolWordToken, exprNode);
            }

            // Method
            var args = new List<CoolAstNode>();
            var nextToken = NextToken();
            while (!nextToken.Type.Is(TokenType.RightParen))
            {
                var formalParser = new CoolFormalParser(this);
                NextToken();
                var formalNode = formalParser.Parse();
                if (formalNode != null)
                    args.Add(formalNode);

                nextToken = Synchronize(new SortedSet<TokenType>() { TokenType.Comma, TokenType.RightParen });
            }

            NextToken();
            Synchronize(new SortedSet<TokenType>() { TokenType.Colon });
            NextToken();
            var rtToken = Synchronize(new SortedSet<TokenType>() { TokenType.TypeId });
            NextToken();
            Synchronize(new SortedSet<TokenType>() { TokenType.LeftBracket });

            var bodyParser = new CoolExprParser(this);
            NextToken();
            var bodyNode = bodyParser.Parse();
            Synchronize(new SortedSet<TokenType>() {TokenType.RightBracket});
            NextToken();

            return new CoolMethodNode(token as CoolWordToken, args, rtToken as CoolWordToken, bodyNode);
        }
    }
}