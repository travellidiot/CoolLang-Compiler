using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

using static Compiler.frontend.cool.CoolScanner;

namespace Compiler.frontend.cool.parsers
{
    public class CoolExprParser : CoolTdParser
    {
        public CoolExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolExprParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>()
            {
                CoolTokenType.Anti,
                CoolTokenType.Not,
                CoolTokenType.Isvoid
            });
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            if (Equals(first.Type, CoolTokenType.ObjectId))
            {
                /* assignment expression or single ID */
                var afterId = Scanner.Lookahead(1) as CoolToken;
                if (afterId == null)
                    throw new NotCoolTokenException(Scanner.Lookahead(1));

                if (afterId.Type.Is(TokenType.Assign))
                {
                    /* assignment expression */
                    var idNode = new CoolIdNode(first as CoolWordToken);
                    NextToken(); // eat id
                    NextToken(); // eat assign
                    var exprParser = new CoolExprParser(this);
                    var expr = exprParser.Parse();
                    return new CoolAssignNode(idNode, expr);
                }
            }
            
            /* Non-ID expression or single Id, current token: first */
            var notExprParser = new CoolNotExprParser(this);
            return notExprParser.Parse();
        }
    }
}