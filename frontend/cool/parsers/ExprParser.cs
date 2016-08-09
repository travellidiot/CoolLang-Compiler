using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;
using static Compiler.frontend.cool.CoolScanner;

namespace Compiler.frontend.cool.parsers
{
    public class ExprParser : TdParser
    {
        public ExprParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public ExprParser(TdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var syncSet = ValueParser.ValueFirstSet;
            var firstSet = new SortedSet<ITokenType>(syncSet);
            var first = Synchronize(firstSet);

            if (Equals(first.Type, CoolTokenType.ObjectId))
            {
                /* assignment expression or single ID */
                var afterId = Scanner.Lookahead(1) as Token;
                if (afterId == null)
                    throw new NotCoolTokenException(Scanner.Lookahead(1));

                if (afterId.Type.Is(TokenType.Assign))
                {
                    /* assignment expression */
                    var idNode = new IdNode(first as WordToken)
                    {
                        LineNumber = first.LineNumber,
                        Position = first.Position
                    };
                    NextToken(); // eat id
                    var assignLine = CurrentToken().LineNumber;
                    var assignPos = CurrentToken().Position;
                    NextToken(); // eat assign
                    var exprParser = new ExprParser(this);
                    var expr = exprParser.Parse();
                    return new AssignNode(idNode, expr)
                    {
                        LineNumber = assignLine,
                        Position = assignPos
                    };
                }
            }
            
            /* Non-ID expression or single Id, current token: first */
            var notExprParser = new NotExprParser(this);
            return notExprParser.Parse();
        }
    }
}