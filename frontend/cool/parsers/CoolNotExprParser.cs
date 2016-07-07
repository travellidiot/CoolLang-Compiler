using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolNotExprParser : CoolTdParser
    {
        public CoolNotExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolNotExprParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>() { CoolTokenType.Not });
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            if (Equals(first.Type, CoolTokenType.Not))
            {
                NextToken(); // eat "not"
                var parser = new CoolNotExprParser(this);
                var expr = parser.Parse();
                return new CoolNotNode(expr);
            }

            var ralParser = new CoolRalExprParser(this);
            return ralParser.Parse();
        }
    }
}