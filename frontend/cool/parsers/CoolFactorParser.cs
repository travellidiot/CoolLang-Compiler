using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolFactorParser : CoolTdParser
    {
        public CoolFactorParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFactorParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<ITokenType>()
            {
                CoolTokenType.Isvoid,
                CoolTokenType.Anti
            });
            var firstSet = new SortedSet<ITokenType>(syncSet);

            Synchronize(firstSet);
            var termParser = new CoolIsVoidParser(this);
            var termNode = termParser.Parse();

            var current = CurrentToken();
            while (Equals(current.Type, CoolTokenType.Star) || Equals(current.Type, CoolTokenType.Slash))
            {
                NextToken(); // eat op
                var node = termParser.Parse();
                termNode = new CoolFactorNode(termNode, current as CoolSpecialToken, node);
                current = CurrentToken();
            }

            return termNode;
        }
    }
}