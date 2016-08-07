using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class NotExprParser : TdParser
    {
        public NotExprParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public NotExprParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = ValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>() { CoolTokenType.Not });
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            if (Equals(first.Type, CoolTokenType.Not))
            {
                NextToken(); // eat "not"
                var parser = new NotExprParser(this);
                var expr = parser.Parse();
                return new NotNode(expr);
            }

            var ralParser = new RalExprParser(this);
            return ralParser.Parse();
        }
    }
}