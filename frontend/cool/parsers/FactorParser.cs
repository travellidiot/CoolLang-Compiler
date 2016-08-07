using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class FactorParser : TdParser
    {
        public FactorParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public FactorParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = ValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>()
            {
                CoolTokenType.Isvoid,
                CoolTokenType.Anti
            });
            var firstSet = new SortedSet<ITokenType>(syncSet);

            Synchronize(firstSet);
            var termParser = new IsVoidParser(this);
            var termNode = termParser.Parse();

            var current = CurrentToken();
            while (Equals(current.Type, CoolTokenType.Star) || Equals(current.Type, CoolTokenType.Slash))
            {
                NextToken(); // eat op
                var node = termParser.Parse();
                termNode = new FactorNode(termNode, current as SpecialToken, node);
                current = CurrentToken();
            }

            return termNode;
        }
    }
}