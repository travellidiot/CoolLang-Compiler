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
    public class CoolFeatureParser : CoolTdParser
    {
        public CoolFeatureParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFeatureParser(CoolTdParser parent) : base(parent)
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
                    return new CoolAttrNode(token as CoolWordToken, typeToken as CoolWordToken);

                NextToken(); // eat "<-"
                var exprParser = new CoolExprParser(this);
                var exprNode = exprParser.Parse();

                return new CoolAttrNode(token as CoolWordToken, typeToken as CoolWordToken, exprNode);
            }

            // Method
            var formalParser = new CoolFormalParser(this);
            var args = new List<CoolFormalNode>();
            if (!Equals(NextToken().Type, CoolTokenType.RightParen))
            {
                // eat "("
                var formalNode = formalParser.Parse() as CoolFormalNode;
                if (formalNode != null)
                {
                    args.Add(formalNode);
                }
            }

            while (!Equals(CurrentToken().Type, CoolTokenType.RightParen))
            {
                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma });
                NextToken(); // eat ","
                var formalNode = formalParser.Parse() as CoolFormalNode;
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

            var bodyParser = new CoolExprParser(this);
            NextToken(); // eat "{"
            var bodyNode = bodyParser.Parse();
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.RightBracket});
            NextToken(); // eat "}"

            ScopeStack.Pop();

            return new CoolMethodNode(token as CoolWordToken, args, rtToken as CoolWordToken, bodyNode);
        }
    }
}