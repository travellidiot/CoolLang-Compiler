using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class RalExprParser : TdParser
    {
        public RalExprParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public RalExprParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var simExprParser = new SimpleExprParser(this);
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
            //simExprParser = new SimpleExprParser(this);
            var node = simExprParser.Parse();
            exprNode = new RalExprNode(exprNode, current as SpecialToken, node);

            return exprNode;
        }
    }
}