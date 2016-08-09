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
            var syncSet = ValueParser.ValueFirstSet;
            var first = Synchronize(syncSet);

            var parser = new AntiTermParser(this);
            if (!Equals(first.Type, CoolTokenType.Isvoid))
                return parser.Parse();

            NextToken(); // eat "isvoid"
            var expr = parser.Parse();
            return new IsVoidNode(expr)
            {
                LineNumber = first.LineNumber,
                Position = first.Position 
            };
        }
    }
}