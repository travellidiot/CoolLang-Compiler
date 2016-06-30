using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolIsVoidParser : CoolTDParser
    {
        public CoolIsVoidParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolIsVoidParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<TokenType>() { TokenType.Isvoid });
            var firstSet = new SortedSet<TokenType>(syncSet); var first = Synchronize(firstSet);

            var parser = new CoolAntiTermParser(this);
            if (first.Type.Is(TokenType.Isvoid))
            {
                NextToken();
                var expr = parser.Parse();
                return new CoolIsVoidNode(expr);
            }

            return parser.Parse();
        }
    }
}