using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class IsVoidParser : TdParser
    {
        public IsVoidParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public IsVoidParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = ValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>() { CoolTokenType.Isvoid });
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            var parser = new AntiTermParser(this);
            if (!Equals(first.Type, CoolTokenType.Isvoid))
                return parser.Parse();

            NextToken(); // eat "isvoid"
            var expr = parser.Parse();
            return new IsVoidNode(expr);
        }
    }
}