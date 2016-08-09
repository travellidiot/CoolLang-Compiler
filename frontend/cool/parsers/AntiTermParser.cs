using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class AntiTermParser : TdParser
    {
        public AntiTermParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public AntiTermParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = ValueParser.ValueFirstSet;
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            if (Equals(first.Type, CoolTokenType.Anti))
            {
                NextToken(); // eat "~"
                var antiParser = new AntiTermParser(this);
                var termNode = antiParser.Parse();
                return new AntiNode(termNode)
                {
                    LineNumber = first.LineNumber,
                    Position = first.Position
                };
            }

            var termParser = new TermParser(this);
            return termParser.Parse();
        }
    }
}