using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolExprParser : CoolTDParser
    {
        public CoolExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolExprParser(CoolTDParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var simExprParser = new CoolSimpleExprParser(this);
            IAstNode exprNode = simExprParser.Parse();

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
            exprNode = new CoolExprNode(exprNode, current as CoolSpecialToken, node);

            return exprNode;
        }
    }
}