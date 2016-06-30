using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolRalExprParser : CoolTdParser
    {
        public CoolRalExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolRalExprParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var simExprParser = new CoolSimpleExprParser(this);
            var exprNode = simExprParser.Parse();

            var current = CurrentToken();
            var opType = current.Type;
            var opSet = new SortedSet<ITokenType>()
            {
                CoolTokenType.LessThan,
                CoolTokenType.LessEqual,
                CoolTokenType.GreatThan,
                CoolTokenType.GreatEqual,
                CoolTokenType.Equal
            };

            if (!opSet.Contains(opType))
                return exprNode;

            NextToken(); // eat op
            //simExprParser = new CoolSimpleExprParser(this);
            var node = simExprParser.Parse();
            exprNode = new CoolRalExprNode(exprNode, current as CoolSpecialToken, node);

            return exprNode;
        }
    }
}