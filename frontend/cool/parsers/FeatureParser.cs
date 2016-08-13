using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    /*
     * feature ::= ID([formal 【, formal】*]) : TYPE { expr }
	 *           | ID : TYPE [<- expr]
     */
    public class FeatureParser : TdParser
    {
        public FeatureParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public FeatureParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var token = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.ObjectId });
            NextToken();
            var afterId = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.LeftParen, CoolTokenType.Colon });

            // Attribute
            if (Equals(afterId.Type, CoolTokenType.Colon))
            {
                NextToken(); // eat ":"
                var typeToken = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId });
                NextToken(); // eat "TYPE"

         
                

                if (!Equals(CurrentToken().Type, CoolTokenType.Assign))
                    return new AttrNode(token as WordToken, typeToken as WordToken)
                    {
                        LineNumber = token.LineNumber,
                        Position = token.Position
                    };

                NextToken(); // eat "<-"
                var exprParser = new ExprParser(this);
                var exprNode = exprParser.Parse();

                return new AttrNode(token as WordToken, typeToken as WordToken, exprNode)
                {
                    LineNumber = token.LineNumber,
                    Position = token.Position
                };
            }

            // Method
            var formalParser = new FormalParser(this);
            var args = new List<FormalNode>();
            if (!Equals(NextToken().Type, CoolTokenType.RightParen))
            {
                // eat "("
                var formalNode = formalParser.Parse() as FormalNode;
                if (formalNode != null)
                {
                    args.Add(formalNode);
                }
            }

            while (!Equals(CurrentToken().Type, CoolTokenType.RightParen))
            {
                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma });
                NextToken(); // eat ","
                var formalNode = formalParser.Parse() as FormalNode;
                if (formalNode != null)
                {
                    args.Add(formalNode);
                }
            }

            NextToken(); // eat ")"
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Colon });
            NextToken(); // eat ":"
            var rtToken = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId });
            NextToken(); // eat "TYPE"
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.LeftBracket });

            var bodyParser = new ExprParser(this);
            NextToken(); // eat "{"
            var bodyNode = bodyParser.Parse();
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.RightBracket});
            NextToken(); // eat "}"

            return new MethodNode(token as WordToken, args, rtToken as WordToken, bodyNode)
            {
                LineNumber = token.LineNumber,
                Position = token.Position
            };
        }
    }
}