using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    /*
     * feature ::= ID([formal 【, formal】*]) : TYPE { expr }
	 *           | ID : TYPE [<- expr]
     */
    public class CoolFeatureParser : CoolTDParser
    {
        public CoolFeatureParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFeatureParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var token = Synchronize(new SortedSet<TokenType>() { TokenType.ObjectId, TokenType.FuncId });
            NextToken();
            //var flwToken = Synchronize(new SortedSet<TokenType>() { TokenType.LeftParen, TokenType.Colon });

            // Attribute
            if (token.Type.Is(TokenType.ObjectId))
            {
                Synchronize(new SortedSet<TokenType>() { TokenType.Colon });
                NextToken();
                var typeToken = Synchronize(new SortedSet<TokenType>() { TokenType.TypeId });
                NextToken();
                var ctnToken = Synchronize(new SortedSet<TokenType>() { TokenType.Assign, TokenType.Semic });
                IAstNode exprNode = null;
                if (ctnToken.Type.Is(TokenType.Assign))
                {
                    NextToken();
                    var exprParser = new CoolExprParser(this);
                    exprNode = exprParser.Parse();
                }
                
                return new CoolAttrNode(token as CoolWordToken, typeToken as CoolWordToken, exprNode);
            }

            // Method
            var formalParser = new CoolFormalParser(this);
            var args = new List<IAstNode>();
            if (!NextToken().Type.Is(TokenType.RightParen))
            {
                var formalNode = formalParser.Parse();
                if (formalNode != null)
                    args.Add(formalNode);
            }

            while (!CurrentToken().Type.Is(TokenType.RightParen))
            {
                Synchronize(new SortedSet<TokenType>() { TokenType.Comma });
                NextToken();
                var formalNode = formalParser.Parse();
                if (formalNode != null)
                    args.Add(formalNode);
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