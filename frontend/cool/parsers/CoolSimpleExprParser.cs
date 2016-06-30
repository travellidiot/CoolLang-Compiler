using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolSimpleExprParser : CoolTdParser
    {
        public CoolSimpleExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolSimpleExprParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var factorParser = new CoolFactorParser(this);
            var factorNode = factorParser.Parse();

            var current = CurrentToken();
            while (Equals(current.Type, CoolTokenType.Add) || Equals(current.Type, CoolTokenType.Minus))
            {
                NextToken(); // eat op
                var node = factorParser.Parse();
                factorNode = new CoolSimpleExprNode(factorNode, current as CoolSpecialToken, node);
                current = CurrentToken();
            }

            return factorNode;
        }
    }
}