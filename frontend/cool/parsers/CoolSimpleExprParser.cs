using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolSimpleExprParser : CoolTDParser
    {
        public CoolSimpleExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolSimpleExprParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var factorParser = new CoolFactorParser(this);
            IAstNode factorNode = factorParser.Parse();

            var current = CurrentToken();
            while (current.Type.Is(TokenType.Add) || current.Type.Is(TokenType.Minus))
            {
                NextToken();
                var node = factorParser.Parse();
                factorNode = new CoolSimpleExprNode(factorNode, current as CoolSpecialToken, node);
                current = CurrentToken();
            }

            return factorNode;
        }
    }
}