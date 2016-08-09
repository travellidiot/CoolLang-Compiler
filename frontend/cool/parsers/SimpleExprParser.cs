using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class SimpleExprParser : TdParser
    {
        public SimpleExprParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public SimpleExprParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var factorParser = new FactorParser(this);
            var factorNode = factorParser.Parse();

            var current = CurrentToken();
            while (Equals(current.Type, CoolTokenType.Add) || Equals(current.Type, CoolTokenType.Minus))
            {
                NextToken(); // eat op
                var node = factorParser.Parse();
                factorNode = new SimpleExprNode(factorNode, current as SpecialToken, node)
                {
                    LineNumber = current.LineNumber,
                    Position = current.Position
                };
                current = CurrentToken();
            }

            return factorNode;
        }
    }
}