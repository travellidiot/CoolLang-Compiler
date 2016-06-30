using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolAntiTermParser : CoolTdParser
    {
        public CoolAntiTermParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolAntiTermParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>() { CoolTokenType.Not });
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            if (Equals(first.Type, CoolTokenType.Anti))
            {
                NextToken(); // eat "~"
                var antiParser = new CoolAntiTermParser(this);
                var termNode = antiParser.Parse();
                return new CoolAntiNode(termNode);
            }

            var termParser = new CoolTermParser(this);
            return termParser.Parse();
        }
    }
}