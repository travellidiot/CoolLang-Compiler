using System.Collections.Generic;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolProgramParser : CoolTDParser
    {
        public CoolProgramParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolProgramParser(CoolTDParser parent) : base(parent)
        {
        }

        // program ::= 【class;】+
        public override IAstNode Parse()
        {
            var classes = new List<IAstNode>();

            Synchronize(new SortedSet<TokenType>() {TokenType.Class});
            var parser = new CoolClassParser(this);
            var clsNode = parser.Parse();
            classes.Add(clsNode);

            Synchronize(new SortedSet<TokenType>() {TokenType.Semic});
            NextToken();

            while (CurrentToken().GetType() != typeof (EofToken))
            {
                clsNode = parser.Parse();
                classes.Add(clsNode);

                Synchronize(new SortedSet<TokenType>() { TokenType.Semic });
                NextToken();
            }

            return new CoolProgramNode(classes);
        }
    }
}