using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolIsVoidParser : CoolTdParser
    {
        public CoolIsVoidParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolIsVoidParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>() { CoolTokenType.Isvoid });
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            var parser = new CoolAntiTermParser(this);
            if (!Equals(first.Type, CoolTokenType.Isvoid))
                return parser.Parse();

            NextToken(); // eat "isvoid"
            var expr = parser.Parse();
            return new CoolIsVoidNode(expr);
        }
    }
}