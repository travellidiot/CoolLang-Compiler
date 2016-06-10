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
            //Synchronize(CoolValueParser.ValueFirstSet);
            var termParser = new CoolTermParser(this);
            IAstNode termNode = termParser.Parse();

            var current = CurrentToken();
            while (current.Type.Is(TokenType.Star) || current.Type.Is(TokenType.Slash))
            {
                NextToken();
                termParser = new CoolTermParser(this);
                var node = termParser.Parse();
                termNode = new CoolFactorNode(termNode, current as CoolSpecialToken, node);
            }

            return termNode;
        }
    }
}