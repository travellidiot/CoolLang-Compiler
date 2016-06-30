using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolAntiTermParser : CoolTDParser
    {
        public CoolAntiTermParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolAntiTermParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<TokenType>() { TokenType.Not });
            var firstSet = new SortedSet<TokenType>(syncSet);
            var first = Synchronize(firstSet);

            var termParser = new CoolTermParser(this);
            if (first.Type.Is(TokenType.Anti))
            {
                NextToken();
                var termNode = termParser.Parse();
                return new CoolAntiNode(termNode);
            }

            return termParser.Parse();
        }
    }
}