using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolRalExprParser : CoolTDParser
    {
        public CoolRalExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolRalExprParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var simExprParser = new CoolSimpleExprParser(this);
            var exprNode = simExprParser.Parse();

            var current = CurrentToken();
            var opType = current.Type.CoolType;
            var opSet = new SortedSet<TokenType>()
            {
                TokenType.LessEqual,
                TokenType.LessEqual,
                TokenType.GreatThan,
                TokenType.GreatEqual,
                TokenType.Equal
            };

            if (!opSet.Contains(opType))
                return exprNode;

            NextToken();
            //simExprParser = new CoolSimpleExprParser(this);
            var node = simExprParser.Parse();
            exprNode = new CoolRalExprNode(exprNode, current as CoolSpecialToken, node);

            return exprNode;
        }
    }
}