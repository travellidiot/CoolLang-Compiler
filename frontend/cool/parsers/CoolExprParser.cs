using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolExprParser : CoolTDParser
    {
        public CoolExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolExprParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<TokenType>()
            {
                TokenType.Anti,
                TokenType.Not,
                TokenType.Isvoid
            });
            var firstSet = new SortedSet<TokenType>(syncSet);
            var first = Synchronize(firstSet);
            var parser = new CoolNotExprParser(this);

            if (first.Type.Is(TokenType.ObjectId))
            {
                var idNode = new CoolIdNode(first as CoolWordToken);
                var flwToken = NextToken();
                if (flwToken.Type.Is(TokenType.Assign))
                {
                    NextToken();
                    var exprParser = new CoolExprParser(this);
                    var expr = exprParser.Parse();
                    return new CoolAssignNode(idNode, expr);
                }

                return idNode;
            }
            
            return parser.Parse();
        }
    }
}