using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolNotExprParser : CoolTDParser
    {
        public CoolNotExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolNotExprParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<TokenType>() { TokenType.Not });
            var firstSet = new SortedSet<TokenType>(syncSet);
            var first = Synchronize(firstSet);

            var parser = new CoolRalExprParser(this);
            if (first.Type.Is(TokenType.Not))
            {
                NextToken();
                var expr = parser.Parse();
                return new CoolNotNode(expr);
            }

            return parser.Parse();
        }
    }
}