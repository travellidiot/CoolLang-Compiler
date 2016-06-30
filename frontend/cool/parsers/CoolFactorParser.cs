using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolFactorParser : CoolTDParser
    {
        public CoolFactorParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFactorParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = CoolValueParser.ValueFirstSet.Union(new SortedSet<TokenType>()
            {
                TokenType.Isvoid,
                TokenType.Anti
            });
            var firstSet = new SortedSet<TokenType>(syncSet);

            Synchronize(firstSet);
            var termParser = new CoolIsVoidParser(this);
            IAstNode termNode = termParser.Parse();

            var current = CurrentToken();
            while (current.Type.Is(TokenType.Star) || current.Type.Is(TokenType.Slash))
            {
                NextToken();
                var node = termParser.Parse();
                termNode = new CoolFactorNode(termNode, current as CoolSpecialToken, node);
            }

            return termNode;
        }
    }
}